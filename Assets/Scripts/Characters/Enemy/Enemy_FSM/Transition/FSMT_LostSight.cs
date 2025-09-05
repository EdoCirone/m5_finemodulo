using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMT_LostSight : AbstractFSMTransition
{
    [SerializeField] private CharacterDetector _detector;
    private bool _hasStoredPosition = false;

    public override bool IsConditionMet(FSMController controller, AbstractFSMState ownerState)
    {
        if (_detector == null)
        {
            _detector = controller.GetComponentInParent<CharacterDetector>();
            if (_detector == null) return false;
        }

        if (!_detector.CanSeeTarget())
        {
            var memory = controller.GetComponentInParent<EnemyMemory>();
            if (memory != null && !_hasStoredPosition)
            {
                Debug.Log(" TRANSIZIONE LostSight ATTIVATA");

                memory.LastKnownPlayerPosition = _detector.Target.position;
                memory.TimeLostPlayer = Time.time;

                _hasStoredPosition = true; 
            }

            return true;
        }

        _hasStoredPosition = false;
        return false;
    }
}
