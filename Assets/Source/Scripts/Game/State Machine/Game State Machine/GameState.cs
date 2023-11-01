using BikeDefied.FSM.GameWindow;

namespace BikeDefied.FSM.Game
{
    public abstract class GameState : State<GameStateMachine>
    {
        public GameState(WindowStateMachine machine) =>
            Machine = machine;

        protected WindowStateMachine Machine { get; private set; }
    }
}
