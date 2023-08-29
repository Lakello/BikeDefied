using Reflex.Attributes;

public class PlayState : GameState
{
    private PlayerInput _input;

    public override void Enter()
    {
        StateMachine.SetWindow<PlayWindowState>();
        _input.Enable();
    }

    public override void Exit()
    {
        _input.Disable();
    }

    [Inject]
    private void Inject(PlayerInput input)
    {
        _input = input;
    }
}