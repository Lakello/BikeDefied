using System;
using System.Collections.Generic;
using BikeDefied.FSM.GameWindow;

namespace BikeDefied.FSM.Game
{
    public class GameStateMachine : StateMachine<GameStateMachine>
    {
        public GameStateMachine(
            WindowStateMachine windowStateMachine,
            Func<Dictionary<Type, State<GameStateMachine>>> getStates)
            : base(getStates) =>
            WindowStateMachine = windowStateMachine;

        public WindowStateMachine WindowStateMachine { get; }
    }
}