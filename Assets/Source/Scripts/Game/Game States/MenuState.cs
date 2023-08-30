using IJunior.StateMachine;
using UnityEngine;

public class MenuState : GameState
{
    public override void Enter()
    {
        Debug.Log("Menu State enter");
        StateMachine.SetWindow<MenuWindowState>();
    }

    public override void Exit(){ Debug.Log("Menu State exit"); }
}