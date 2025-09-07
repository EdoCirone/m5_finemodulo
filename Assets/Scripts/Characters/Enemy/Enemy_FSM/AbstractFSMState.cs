using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractFSMState : MonoBehaviour
{

    protected FSMController _controller;

    protected AbstractFSMTransition[] _transitions;

    public abstract void StateEnter();

    public abstract void StateExit();

    public abstract void StateFixedUpdate();

    public abstract void StateUpdate();

    public virtual void Setup(FSMController controller)
    {
        _controller = controller;
        _transitions = GetComponents<AbstractFSMTransition>();
    }

    public AbstractFSMState EvaluateTransitions()
    {
        foreach (AbstractFSMTransition transition in _transitions)
        {
            if (transition.IsConditionMet(_controller, this))
            {
                return transition.GetTargetState();
            }

        }
        return null;

    }
}
