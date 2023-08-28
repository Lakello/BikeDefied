public class PCInputHandler : IInputHandler
{
    private PlayerInput _input;

    public PCInputHandler(PlayerInput input)
    {
        _input = input;
    }

    public float Horizontal => _input.PC.Horizontal.ReadValue<float>();
}
