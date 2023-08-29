﻿using Reflex.Attributes;

public class MenuState : GameState
{
    public override void Enter()
    {
        StateMachine.SetWindow<MenuWindowState>();
    }

    public override void Exit(){}
}