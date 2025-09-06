using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMS_LookingAround : AbstractFSMState
{
    private EnemyFSMAnimationController _animator;

    public override void Setup(FSMController controller)
    {
        base.Setup(controller);

        _animator = controller.GetComponentInParent<EnemyFSMAnimationController>();
    }

    public override void StateEnter()
    {
        _animator?.SetState(ANIMSTATE.LOOKINGAROUND);

        Debug.Log(" Sentinella in allerta!");
    }
    public override void StateExit() { }
    public override void StateUpdate() { }
    public override void StateFixedUpdate() { }


}