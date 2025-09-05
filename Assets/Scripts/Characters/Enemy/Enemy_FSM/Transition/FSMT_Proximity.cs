using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum COMPARISON_TYPE { LESS_THAN, GREATER_THAN }

public class FSMT_Proximity : AbstractFSMTransition
{
    [SerializeField] private Transform _target;
    [SerializeField] private COMPARISON_TYPE _type;
    [SerializeField] private float _tresholdDistace = 5;

    public override bool IsConditionMet(FSMController controller, AbstractFSMState ownerState)
    {
        float sqrDistance = (controller.transform.position - _target.position).sqrMagnitude;
        float sqrTresholdDistance = _tresholdDistace * _tresholdDistace;

        switch (_type)
        {

            default:
            case COMPARISON_TYPE.LESS_THAN: return sqrDistance < sqrTresholdDistance;
            case COMPARISON_TYPE.GREATER_THAN: return sqrDistance > sqrTresholdDistance;

        }
    }

}
