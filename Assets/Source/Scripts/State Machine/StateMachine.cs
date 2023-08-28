using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected Dictionary<Type, State> States;
    protected State CurrentState;

    private void Awake()
    {
        States = new Dictionary<Type, State>();
    }

    protected abstract void Start();

    private void OnDisable()
    {
        CurrentState?.Exit();
    }

    public void EnterIn<TState>() where TState : State
    {
        if (States.ContainsKey(typeof(TState)) == false)
            throw new NullReferenceException(nameof(States));

        if (States.TryGetValue(typeof(TState), out State state))
        {
            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();
        }
    }

    public void AddState(State state)
    {
        Type type = state.GetType();

        if (States.ContainsKey(type) == false)
        {
            States.Add(type, state);
        }
    }
}