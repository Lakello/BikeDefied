using IJunior.StateMachine;
using System;
using UnityEngine;

public class PlayState : GameState
{
    public override void Enter()
    {
        Debug.Log("Play State enter");
        StateMachine.SetWindow<PlayWindowState>();
        InputHandler.Instance.Input.Enable();
    }

    public override void Exit()
    {
        Debug.Log("Play State exit");
        InputHandler.Instance.Input.Disable();
    }
}