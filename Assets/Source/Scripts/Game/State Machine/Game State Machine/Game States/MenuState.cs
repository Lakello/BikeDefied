using BikeDefied.FSM.GameWindow;
using BikeDefied.FSM.GameWindow.States;
using System;

namespace BikeDefied.FSM.Game.States
{
    public class MenuState : GameState, IMenuStateChangeble
    {
        public MenuState(WindowStateMachine machine) : base(machine) { }

        public event Func<bool> StateChanged;

        public override void Enter()
        {
            WindowStateMachine.EnterIn<MenuWindowState>();
            StateChanged?.Invoke();
        }

        public override void Exit() { }
    }
}