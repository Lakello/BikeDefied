using BikeDefied.FSM.GameWindow;
using BikeDefied.FSM.GameWindow.States;
using System;

namespace BikeDefied.FSM.Game.States
{
    public class MenuState : GameState, IGameMenu
    {
        public MenuState(WindowStateMachine machine) : base(machine) { }

        public event Action GameMenu;

        public override void Enter()
        {
            Machine.EnterIn<MenuWindowState>();
            GameMenu?.Invoke();
        }

        public override void Exit() { }
    }
}