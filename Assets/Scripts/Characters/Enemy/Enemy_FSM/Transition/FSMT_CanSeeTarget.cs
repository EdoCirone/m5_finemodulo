using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMT_CanSeeTarget : AbstractFSMTransition
{

    private CharacterDetector _detector;

    public override bool IsConditionMet(FSMController controller, AbstractFSMState ownerState)
    {
        if (_detector == null)
        {
            _detector = controller.GetComponent<CharacterDetector>();
            if (_detector == null) return false;
        }

        if (_detector.CanSeeTarget())
        {
            var memory = controller.GetComponent<EnemyMemory>();
            if (memory != null && _detector.Target != null)
            {
                memory.FirstSightPosition = controller.transform.position;
                memory.LastKnownPlayerPosition = _detector.Target.position;
            }

            return true;
        }

        return false;
    }


}