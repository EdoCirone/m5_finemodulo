using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMS_Allert : AbstractFSMState
{
    private EnemyFSMAnimationController _animator;

    public override void StateEnter()
    {
        _animator = GetComponentInParent<EnemyFSMAnimationController>();
        _animator?.SetState(ANIMSTATE.ALERT);

        Debug.Log(" Sentinella in allerta!");
    }
    public override void StateExit() { }
    public override void StateUpdate() { }
    public override void StateFixedUpdate() { }
 


}
