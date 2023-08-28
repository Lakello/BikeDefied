public abstract class GameStateMachine : StateMachine
{
    protected override void Start()
    {
        EnterIn<MenuState>();
    }
}