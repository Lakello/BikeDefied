namespace BikeDefied.FSM.GameWindow
{
    public abstract class WindowState : State<WindowStateMachine>
    {
        private Window _window;

        public void Init(Window window) => _window = window;

        public override void Enter()
        {
            _window.gameObject.SetActive(true);
        }

        public override void Exit()
        {
            _window.gameObject.SetActive(false);
        }
    }
}