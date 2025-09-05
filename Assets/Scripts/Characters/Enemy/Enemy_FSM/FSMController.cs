using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMController : MonoBehaviour
{


    [SerializeField] private AbstractFSMState _defaultState;
    [SerializeField] private float _currentStateTime;
    [SerializeField] private AbstractFSMState _currentState;
   
    public float CurrentStateTime => _currentStateTime;
 
    
    private void Awake()
    {
        AbstractFSMState[] availableStates = GetComponentsInChildren<AbstractFSMState>(true);

        if (availableStates.Length == 0)
        {

            Debug.LogError($"The FSMController of {gameObject.name} has no AbstractFSMStates as Children", gameObject);

        }

        foreach (AbstractFSMState state in availableStates)
        {
            state.Setup(this);
        }

        if (_defaultState != null)
        {
            SetState(_defaultState);
        }

        else
        {
            SetState(availableStates[0]);
        }


    }


    private void Update()
    {
        if (_currentState != null)

        {
            _currentStateTime += Time.deltaTime;

            _currentState.StateUpdate();

            AbstractFSMState targetState = _currentState.EvalutateTransitions();

            if (targetState != null) { SetState(targetState); }
        }
    }

    private void FixedUpdate()
    {
        if (_currentState != null)
        {
            _currentState.StateFixedUpdate();

        }
    }

    public void SetState(AbstractFSMState state)
    {
        if (_currentState != null)
        {
            _currentState.StateExit();
        }

        _currentState = state;
        _currentStateTime = 0f;
        _currentState.StateEnter();
    }

}
