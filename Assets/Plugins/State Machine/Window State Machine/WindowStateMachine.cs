using System;
using System.Collections.Generic;

namespace IJunior.StateMachine
{
    public class WindowStateMachine : StateMachine<WindowStateMachine>
    {
        public static WindowStateMachine Instance { get; private set; }

        protected override WindowStateMachine SelfType => this;

        public WindowStateMachine(Func<Dictionary<Type, State<WindowStateMachine>>> getStates) : base(getStates) => Instance ??= this;

        public TState GetState<TState>(Window window) where TState : State<WindowStateMachine>
        {
            if (States.TryGetValue(window.WindowType, out State<WindowStateMachine> state))
                return (TState)state;
            else
                return null;
        }
    }
}