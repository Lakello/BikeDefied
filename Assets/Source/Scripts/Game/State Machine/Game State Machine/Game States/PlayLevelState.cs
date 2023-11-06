using BikeDefied.FSM.GameWindow;
using BikeDefied.FSM.GameWindow.States;
using System;

namespace BikeDefied.FSM.Game.States
{
    public class PlayLevelState : GameState, IPlayLevelStateChangeble
    {
        private readonly PlayerInput _playerInput;

        public PlayLevelState(PlayerInput input, WindowStateMachine machine) : base(machine) =>
            _playerInput = input;

        public event Func<bool> StateChanged;

        public override void Enter()
        {
            WindowStateMachine.EnterIn<PlayWindowState>();
            _playerInput.Enable();

            StateChanged?.Invoke();
        }

        public override void Exit() =>
            _playerInput.Disable();
    }
}