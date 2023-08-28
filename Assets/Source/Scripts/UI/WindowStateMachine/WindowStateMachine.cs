public class WindowStateMachine : StateMachine
{
    protected override void Start()
    {
        EnterIn<MenuWindowState>();
    }
}