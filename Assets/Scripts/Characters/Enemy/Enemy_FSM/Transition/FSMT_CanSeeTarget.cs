using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMT_CanSeeTarget : AbstractFSMTransition
{

    private CharacterDetector _detector;

    private void Awake()
    {
        _detector = GetComponentInParent<CharacterDetector>();
    }

    public override bool IsConditionMet(FSMController controller, AbstractFSMState ownerState)
    {
        if (_detector == null) return false;

        if (_detector.CanSeeTarget())
        {
  

            var memory = controller.GetComponentInParent<EnemyMemory>();
            if (memory != null && !memory.FirstSightPosition.HasValue)
            {
                memory.FirstSightPosition = _detector.Target.position;
                memory.FirstSightRotation = _detector.Target.rotation;
                memory.EnemyPositionAtFirstSight = controller.transform.position;
                memory.EnemyRotationAtFirstSight = controller.transform.rotation;
            }

            return true;
        }
        else
        {

            return false;
        }
    }
}