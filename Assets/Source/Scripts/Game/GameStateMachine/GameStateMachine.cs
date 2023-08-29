using Reflex.Attributes;
using System;

public class GameStateMachine : StateMachine<GameStateMachine>
{
    private WindowStateMachine _windowStateMachine;

    protected override void Start()
    {
        EnterIn<MenuState>();
    }

    public void SetWindow<TWindow>() where TWindow : WindowState
    {
        _windowStateMachine.EnterIn<TWindow>();
    }

    [Inject]
    private void Inject(WindowStateMachine windowStateMachine)
    {
        _windowStateMachine = windowStateMachine;
    }
}