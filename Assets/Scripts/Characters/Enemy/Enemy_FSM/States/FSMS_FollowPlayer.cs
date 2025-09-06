using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

public class FSMS_FollowPlayer : AbstractFSMState
{

    [SerializeField] private Transform _target;
    [SerializeField] private float _pathSearchInterval = 0.1f;

    private float _lastPathSearchTime;

    private NavMeshAgent _agent;
    private EnemyMemory _memory;
    private EnemyFSMAnimationController _animator;

    public override void Setup(FSMController controller)
    {
        base.Setup(controller);
        _agent = controller.GetComponentInParent<NavMeshAgent>();
        _memory = controller.GetComponentInParent<EnemyMemory>();
        _animator = controller.GetComponentInParent<EnemyFSMAnimationController>();
    }


    public void UpdatePath()
    {


        _agent.SetDestination(_target.position);
        _lastPathSearchTime = _controller.CurrentStateTime;


    }


    public override void StateEnter()
    {
        if (_target == null) Debug.LogWarning("FSMS_FollowPlayer: target NON assegnato!");


        _controller.TryGetComponent<CharacterDetector>(out var detector);

        if (detector != null && detector.Target != null)
        {
            SetTarget(detector.Target);
        }

        if (_agent == null)
        {
            Debug.LogError($"{name}: NavMeshAgent non trovato!");
            return;
        }


        _agent.isStopped = false;
        _animator?.SetState(ANIMSTATE.RUN);

        UpdatePath();



    }


    public override void StateExit()
    {

        _agent.SetDestination(_agent.transform.position);

    }


    public override void StateFixedUpdate()
    {
    }



    public override void StateUpdate()
    {
        if (_controller.CurrentStateTime - _lastPathSearchTime >= _pathSearchInterval)
        {
            Debug.Log($"?? Seguendo target: {_target.position}");
            UpdatePath();
        }


    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }


}
