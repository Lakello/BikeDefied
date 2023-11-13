using BikeDefied.FSM.GameWindow;

namespace BikeDefied.FSM.Game
{
    public abstract class GameState : State<GameStateMachine>
    {
        protected WindowStateMachine WindowStateMachine { get; private set; }

        protected GameState(WindowStateMachine machine) =>
            WindowStateMachine = machine;
    }
}
