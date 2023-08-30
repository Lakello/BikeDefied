public class PCInputHandler : IInputHandler
{
    public float Horizontal => InputHandler.Instance.Input.PC.Horizontal.ReadValue<float>();
}
