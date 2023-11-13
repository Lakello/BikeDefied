using System;
using System.Collections.Generic;

namespace BikeDefied.FSM.GameWindow
{
    public class WindowStateMachine : StateMachine<WindowStateMachine>
    {
        public WindowStateMachine(Func<Dictionary<Type, State<WindowStateMachine>>> getStates)
            : base(getStates) { }

        public event Action StateUpdated;

        public override void EnterIn<TState>()
        {
            base.EnterIn<TState>();
            StateUpdated?.Invoke();
        }

        public TState TryGetState<TState>(Window window) 
            where TState : State<WindowStateMachine> =>
            (TState)TryGetState(window.WindowType);
    }
}