using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FSMT_ExitTime : AbstractFSMTransition
{


    [SerializeField] private float _duration;
    public override bool IsConditionMet(FSMController controller, AbstractFSMState ownerState)
    {
        bool result = controller.CurrentStateTime >= _duration;
        Debug.Log($"[FSMT_ExitTime] Stato attivo da {controller.CurrentStateTime:F2}s / Soglia: {_duration}s -> {result}");
        return result;
    }
}