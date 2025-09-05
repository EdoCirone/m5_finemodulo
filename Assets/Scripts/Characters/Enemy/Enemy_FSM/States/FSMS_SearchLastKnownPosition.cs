using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMS_SearchLastKnownPosition : AbstractFSMState
{



    private NavMeshAgent _agent;
    private EnemyMemory _memory;


    public override void StateEnter()
    {

        _agent = GetComponentInParent<NavMeshAgent>();
        _memory = GetComponentInParent<EnemyMemory>();

        if (_memory != null && _memory.HasLastKnownPosition)
        {
            Debug.Log($" Sto andando a: {_memory.LastKnownPlayerPosition.Value}");
            _agent.SetDestination(_memory.LastKnownPlayerPosition.Value);
        }


    }
    public override void StateUpdate()
    {
        if (_agent == null || _memory == null) return;

        if (_agent.HasReachedDestination())
        {
            Debug.Log("Raggiunta ultima posizione nota, inizia ricerca...");
        }
    }


    public override void StateExit() { }

    public override void StateFixedUpdate() { }

}
