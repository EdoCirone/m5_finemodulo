using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMT_ExitTime : AbstractFSMTransition
{

    [SerializeField] private float _duration;
    public override bool IsConditionMet(FSMController controller, AbstractFSMState ownerState)
    {
        return controller.CurrentStateTime >= _duration;
    }
}
