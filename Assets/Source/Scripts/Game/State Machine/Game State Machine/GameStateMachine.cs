using BikeDefied.FSM.GameWindow;
using System;
using System.Collections.Generic;

namespace BikeDefied.FSM.Game
{
    public class GameStateMachine : StateMachine<GameStateMachine>
    {
        public WindowStateMachine WindowStateMachine { get; }

        public GameStateMachine(WindowStateMachine windowStateMachine, 
            Func<Dictionary<Type, State<GameStateMachine>>> getStates) 
            : base(getStates) =>
            WindowStateMachine = windowStateMachine;

        public void SetWindow<TWindow>()
            where TWindow : WindowState =>
            WindowStateMachine.EnterIn<TWindow>();

        public TState TryGetState<TState>()
            where TState : State<GameStateMachine> =>
            (TState)TryGetState(typeof(TState));
    }
}