using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMS_SentinelState : AbstractFSMState
{
    [SerializeField] float _rotationSpeed = 180f;

    private NavMeshAgent _agent;
    private EnemyMemory _memory;
    private bool _hasReset = false;

    public override void Setup(FSMController controller)
    {
        base.Setup(controller);
        _agent = GetComponentInParent<NavMeshAgent>();
        _memory = GetComponentInParent<EnemyMemory>();
    }

    public override void StateEnter()
    {
        _hasReset = false;
        Debug.Log("[FSMS_SentinelState] Entrato nello stato sentinella.");
    }

    public override void StateUpdate() //Faccio la rotazione qui, mi costa uno stato aggiuntivo rispetto al wait ma è la soluzione più elegante che ho trovato per farlo tornare alla rotazione d'origine senza maciullare lo stato Return
    {
        if (_memory == null || !_memory.EnemyRotationAtFirstSight.HasValue || _hasReset)
            return;

        Quaternion targetRot = _memory.EnemyRotationAtFirstSight.Value;

        _agent.transform.rotation = Quaternion.RotateTowards(
            _agent.transform.rotation,
            targetRot,
            _rotationSpeed * Time.deltaTime
        );

        float angle = Quaternion.Angle(_agent.transform.rotation, targetRot);
        if (angle < 1f)
        {
            _memory.ClearMemory();
            _hasReset = true;
            Debug.Log("[FSMS_SentinelState] Rotazione completata. Memoria resettata.");
        }
    }

    public override void StateExit() { }
    public override void StateFixedUpdate() { }
}