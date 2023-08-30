using Reflex.Attributes;

namespace IJunior.StateMachine
{
    public class GameStateMachine : StateMachine<GameStateMachine>
    {
        private static GameStateMachine _instance;

        private WindowStateMachine _windowStateMachine;

        protected override void Start()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }

        public void SetWindow<TWindow>() where TWindow : WindowState
        {
            _windowStateMachine.EnterIn<TWindow>();
        }

        [Inject]
        private void Inject(WindowStateMachine windowStateMachine)
        {
            _windowStateMachine = windowStateMachine;
        }
    }
}