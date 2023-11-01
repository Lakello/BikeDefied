using BikeDefied.FSM.GameWindow;
using BikeDefied.FSM.GameWindow.States;
using System;

namespace BikeDefied.FSM.Game.States
{
    public class PlayLevelState : GameState, IGamePlay
    {
        private readonly PlayerInput _playerInput;

        public PlayLevelState(PlayerInput input, WindowStateMachine machine) : base(machine)
        {
            _playerInput = input;
        }

        public event Action GamePlay;

        public override void Enter()
        {
            Machine.EnterIn<PlayWindowState>();
            _playerInput.Enable();

            GamePlay?.Invoke();
        }

        public override void Exit()
        {
            _playerInput.Disable();
        }
    }
}