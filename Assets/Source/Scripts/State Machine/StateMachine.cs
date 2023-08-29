using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class StateMachine<TMachine> : MonoBehaviour where TMachine : StateMachine<TMachine>
{
    protected Dictionary<Type, State<TMachine>> States = new Dictionary<Type, State<TMachine>>();
    protected State<TMachine> CurrentState;

    protected abstract void Start();

    private void OnDisable()
    {
        CurrentState?.Exit();
    }

    public void EnterIn<TState>() where TState : State<TMachine>
    {
        if (States.ContainsKey(typeof(TState)) == false)
            throw new NullReferenceException(nameof(States));

        if (States.TryGetValue(typeof(TState), out State<TMachine> state))
        {
            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();
        }
    }

    public void AddState(State<TMachine> state)
    {
        Type type = state.GetType();

        if (States.ContainsKey(type) == false)
        {
            States.Add(type, state);
        }
    }
}