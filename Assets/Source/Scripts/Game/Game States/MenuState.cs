using IJunior.StateMachine;
using UnityEngine;

public class MenuState : GameState
{
    public override void Enter()
    {
        GameStateMachine.Instance.SetWindow<MenuWindowState>();
    }

    public override void Exit(){}
}