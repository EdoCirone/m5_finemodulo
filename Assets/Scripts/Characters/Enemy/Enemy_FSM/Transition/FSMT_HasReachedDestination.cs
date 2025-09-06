using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class FSMT_HasReachedDestination : AbstractFSMTransition
{


    private NavMeshAgent _agent;
    private void Awake()
    {
        _agent = GetComponentInParent<NavMeshAgent>();
    }


    public override bool IsConditionMet(FSMController controller, AbstractFSMState ownerState)
    {
        if (_agent == null)
        {
            Debug.LogWarning("[FSMT_HasReachedDestination] NavMeshAgent non trovato!");
            return false;
        }


        bool reached = _agent.HasReachedDestination();
        Debug.Log($"[FSMT_HasReachedDestination] Check: HasReachedDestination = {reached}");
        return reached;
    }
}