using IJunior.TypedScenes;
using UnityEngine;

namespace IJunior.StateMachine
{
    public class GameStateMachine : StateMachine<GameStateMachine>, ISceneLoadHandlerState<GameStateMachine>
    {
        [SerializeField] private WindowStateMachine _windowStateMachine;
        
        private static GameStateMachine _instance;

        public static GameStateMachine Instance => _instance;

        protected override GameStateMachine SelfType => this;

        public void SetWindow<TWindow>() where TWindow : WindowState
        {
            _windowStateMachine.EnterIn<TWindow>();
        }

        public void OnSceneLoaded<TState>() where TState : State<GameStateMachine>
        {
            Init(ref _instance);

            _windowStateMachine.Init();

            _instance.EnterIn<TState>();
        }
    }
}