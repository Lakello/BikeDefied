using System;
using System.Collections.Generic;

namespace IJunior.StateMachine
{
    public class WindowStateMachine : StateMachine<WindowStateMachine>
    {
        public static WindowStateMachine Instance { get; private set; }

        public WindowStateMachine(Func<Dictionary<Type, State<WindowStateMachine>>> getStates) : base(getStates) =>
            Instance ??= this;

        public event Action StateUpdated;

        public TState GetState<TState>(Window window) where TState : State<WindowStateMachine>
        {
            if (States.TryGetValue(window.WindowType, out State<WindowStateMachine> state))
                return (TState)state;
            else
                return null;
        }

        public override void EnterIn<TState>()
        {
            base.EnterIn<TState>();
            StateUpdated?.Invoke();
        }
    }
}