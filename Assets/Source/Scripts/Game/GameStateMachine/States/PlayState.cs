using Reflex.Attributes;

public class PlayeState : State
{
    private PlayerInput _input;

    public override void Enter()
    {
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