using BikeDefied.FSM.GameWindow;

namespace BikeDefied.FSM.Game
{
    public abstract class GameState : State<GameStateMachine>
    {
        protected readonly WindowStateMachine WindowStateMachine;

        public GameState(WindowStateMachine machine) =>
            WindowStateMachine = machine;
    }
}
