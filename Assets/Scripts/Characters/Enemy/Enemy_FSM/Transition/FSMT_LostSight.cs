using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMT_LostSight : AbstractFSMTransition
{

    private bool _hasStoredPosition = false;
    private CharacterDetector _detector;

    private void Awake()
    {
        _detector = GetComponentInParent<CharacterDetector>();
    }

    public override bool IsConditionMet(FSMController controller, AbstractFSMState ownerState)
    {
        if (_detector == null)
        {
            _detector = controller.GetComponentInParent<CharacterDetector>();
            if (_detector == null)
            {
                Debug.LogWarning("[FSMT_LostSight] CharacterDetector non trovato!");
                return false;
            }
        }

        if (!_detector.CanSeeTarget())
        {
            Debug.Log("[FSMT_LostSight] Target NON visibile");

            var memory = controller.GetComponentInParent<EnemyMemory>();
            if (memory != null && !_hasStoredPosition)
            {
                Debug.Log("[FSMT_LostSight] Memorizzo ultima posizione conosciuta del player");
                memory.LastKnownPlayerPosition = _detector.Target.position;
                memory.TimeLostPlayer = Time.time;
                _hasStoredPosition = true;
            }

            return true;
        }

        Debug.Log("[FSMT_LostSight] Target visibile, nessuna transizione");
        _hasStoredPosition = false;
        return false;
    }
}
