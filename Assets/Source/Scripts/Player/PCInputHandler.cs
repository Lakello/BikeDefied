public class PCInputHandler : IInputHandler
{
    private PlayerInput _playerInput;

    public float Horizontal => _playerInput.PC.Horizontal.ReadValue<float>();

    public PCInputHandler(PlayerInput input)
    {
        _playerInput = input;
    }
}
