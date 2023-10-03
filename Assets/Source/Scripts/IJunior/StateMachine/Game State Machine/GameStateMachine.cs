using IJunior.TypedScenes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IJunior.StateMachine
{
    public class GameStateMachine : StateMachine<GameStateMachine>
    {
        public static GameStateMachine Instance { get; private set; }

        public GameStateMachine(Func<Dictionary<Type, State<GameStateMachine>>> getStates) : base(getStates) =>
            Instance ??= this;

        public void SetWindow<TWindow>() where TWindow : WindowState
        {
            WindowStateMachine.Instance.EnterIn<TWindow>();
        }

        public TState GetState<TState>() where TState : State<GameStateMachine>
        {
            if (States.TryGetValue(typeof(TState), out State<GameStateMachine> state))
                return (TState)state;
            else
                return null;
        }
    }
}