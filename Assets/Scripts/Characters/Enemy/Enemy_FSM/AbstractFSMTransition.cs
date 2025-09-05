using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractFSMTransition : MonoBehaviour
{
    [SerializeField] protected AbstractFSMState _targetState;

    public AbstractFSMState GetTargetState() => _targetState;

    public abstract bool IsConditionMet(FSMController controller, AbstractFSMState ownerState);
}