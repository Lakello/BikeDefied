namespace IJunior.StateMachine
{
    public class WindowStateMachine : StateMachine<WindowStateMachine>
    {
        private static WindowStateMachine _instance;

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
    }
}