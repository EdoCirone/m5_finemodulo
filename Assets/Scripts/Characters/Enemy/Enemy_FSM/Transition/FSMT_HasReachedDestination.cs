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
        return _agent != null && _agent.HasReachedDestination();
    }


}
