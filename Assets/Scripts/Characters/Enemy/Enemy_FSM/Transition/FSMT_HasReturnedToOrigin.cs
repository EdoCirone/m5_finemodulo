using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMT_HasReturnedToOrigin : AbstractFSMTransition
{
    private NavMeshAgent _agent;
    private EnemyMemory _memory;

    private void Awake()
    {
        _agent = GetComponentInParent<NavMeshAgent>();
        _memory = GetComponentInParent<EnemyMemory>();
    }

    public override bool IsConditionMet(FSMController controller, AbstractFSMState ownerState)
    {
        if (_agent == null || _memory == null || !_memory.EnemyPositionAtFirstSight.HasValue)
            return false;

        float distance = Vector3.Distance(_agent.transform.position, _memory.EnemyPositionAtFirstSight.Value);
        bool result = distance <= _agent.stoppingDistance;


        return result;
    }
}