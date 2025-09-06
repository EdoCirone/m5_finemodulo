using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMS_ReturnToOrigin : AbstractFSMState
{
    private EnemyFSMAnimationController _animator;
    private NavMeshAgent _agent;
    private EnemyMemory _memory;

    private bool _isRotating = true;
    private const float _rotationSpeed = 180f; // gradi/sec

    public override void Setup(FSMController controller)
    {
        base.Setup(controller);
        _agent = controller.GetComponentInParent<NavMeshAgent>();
        _animator = controller.GetComponentInParent<EnemyFSMAnimationController>();
        _memory = controller.GetComponentInParent<EnemyMemory>();
    }

    public override void StateEnter()
    {
        if (_agent == null || _memory == null) return;

        _agent.isStopped = true; // blocca il movimento
        _isRotating = true;

        _animator?.SetState(ANIMSTATE.IDLE); // idle mentre ruota
    }

    public override void StateUpdate()
    {
        if (_agent == null || _memory == null) return;

        if (_isRotating)
        {
            if (_memory.FirstSightRotation.HasValue)
            {
                Quaternion targetRotation = _memory.FirstSightRotation.Value;

                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    _rotationSpeed * Time.deltaTime
                );

                float angle = Quaternion.Angle(transform.rotation, targetRotation);
                if (angle < 1f)
                {
                    // rotazione completata
                    _isRotating = false;

                    if (_memory.FirstSightPosition.HasValue)
                    {
                        _agent.SetDestination(_memory.FirstSightPosition.Value);
                        _agent.isStopped = false;

                        _animator?.SetState(ANIMSTATE.WALK);
                    }
                }
            }
        }
        else
        {
            if (_agent.HasReachedDestination())
            {
                _agent.isStopped = true;
                _animator?.SetState(ANIMSTATE.IDLE);

                // Reset memoria
                _memory.FirstSightPosition = null;
                _memory.FirstSightRotation = null;
                _memory.LastKnownPlayerPosition = null;
            }
        }
    }

    public override void StateExit()
    {
        if (_agent != null)
        {
            _agent.ResetPath();
        }
    }

    public override void StateFixedUpdate() { }
}
