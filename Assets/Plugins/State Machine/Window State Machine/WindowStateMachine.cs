namespace IJunior.StateMachine
{
    public class WindowStateMachine : StateMachine<WindowStateMachine>
    {
        private static WindowStateMachine _instance;

        protected override WindowStateMachine SelfType => this;

        public void Init() => Init(ref _instance);
    }
}