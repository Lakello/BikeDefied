using System;
using System.Collections.Generic;
using BikeDefied.FSM.GameWindow;

namespace BikeDefied.FSM.Game
{
    public class GameStateMachine : StateMachine<GameStateMachine>
    {
        public readonly WindowStateMachine WindowStateMachine;

        public GameStateMachine(WindowStateMachine windowStateMachine, 
                                Func<Dictionary<Type, State<GameStateMachine>>> getStates)
                                    : base(getStates) =>
            WindowStateMachine = windowStateMachine;

        public void SetWindow<TWindow>() where TWindow : WindowState =>
            WindowStateMachine.EnterIn<TWindow>();

        public TState TryGetState<TState>() where TState : State<GameStateMachine> =>
            (TState)TryGetState(typeof(TState));
    }
}