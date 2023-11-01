using BikeDefied.FSM.GameWindow;
using System;
using System.Collections.Generic;

namespace BikeDefied.FSM.Game
{
    public class GameStateMachine : StateMachine<GameStateMachine>
    {
        private WindowStateMachine _windowStateMachine;

        public WindowStateMachine Window => _windowStateMachine;

        public GameStateMachine(WindowStateMachine windowStateMachine, Func<Dictionary<Type, State<GameStateMachine>>> getStates)
            : base(getStates)
        {
            _windowStateMachine = windowStateMachine;
        }

        public void SetWindow<TWindow>() where TWindow : WindowState
        {
            _windowStateMachine.EnterIn<TWindow>();
        }

        public TState TryGetState<TState>() where TState : State<GameStateMachine>
        {
            return (TState)TryGetState(typeof(TState));
        }
    }
}