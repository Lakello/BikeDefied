using IJunior.StateMachine;
using System;

public class MenuState : GameState, IGameMenu
{
    public event Action GameMenu;

    public override void Enter()
    {
        GameStateMachine.Instance.SetWindow<MenuWindowState>();
        GameMenu?.Invoke();
    }

    public override void Exit(){}
}