public class WindowStateMachine : StateMachine<WindowStateMachine>
{
    protected override void Start()
    {
        EnterIn<MenuWindowState>();
    }
}