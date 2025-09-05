using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMS_ReturnToOrigin : AbstractFSMState
{

    private NavMeshAgent _agent;
    private EnemyMemory _memory;

    public override void StateEnter()
    {
        _agent = GetComponentInParent<NavMeshAgent>();
        _memory = GetComponentInParent<EnemyMemory>();

        if (_memory != null && _memory.FirstSightPosition.HasValue)
        {
            _agent.SetDestination(_memory.FirstSightPosition.Value);
        }

    }

    public override void StateExit()
    {

        if (_memory != null)
        {
            _memory.FirstSightPosition = null;
            _memory.LastKnownPlayerPosition = null;
        }

        _agent.ResetPath();

    }
    public override void StateUpdate()
    {
        if (_agent == null || _memory == null) return;

        if (_agent.HasReachedDestination())
        {

        }

    }


    public override void StateFixedUpdate(){}



}
