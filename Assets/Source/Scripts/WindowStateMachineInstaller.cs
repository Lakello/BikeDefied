using BikeDefied.FSM;
using BikeDefied.FSM.Game;
using BikeDefied.FSM.GameWindow;
using BikeDefied.FSM.GameWindow.States;
using BikeDefied.TypedScenes;
using BikeDefied.UI.Buttons;
using Reflex.Core;
using UnityEngine;

namespace BikeDefied
{
    public class WindowStateMachineInstaller : MonoBehaviour, IInstaller, ISceneLoadHandlerOnState<GameStateMachine>
    {
        [SerializeField] private LeaderboardButton _leaderboardButton;
        [SerializeField] private MainMenuButton _backToMainMenuFromLeaderboardButton;

        private TransitionInitializer<WindowStateMachine> _transitionInitializer;
        private WindowStateMachine _windowStateMachine;

        private void OnEnable() =>
            _transitionInitializer?.OnEnable();

        private void OnDisable() =>
            _transitionInitializer?.OnDisable();

        public void InstallBindings(ContainerDescriptor descriptor)
        {
            _transitionInitializer = new TransitionInitializer<WindowStateMachine>(_windowStateMachine);

            _transitionInitializer.InitTransition<LeaderboardWindowState>(_leaderboardButton);
            _transitionInitializer.InitTransition<MenuWindowState>(_backToMainMenuFromLeaderboardButton);
        }

        public void OnSceneLoaded<TState>(GameStateMachine machine)
            where TState : State<GameStateMachine> =>
            _windowStateMachine = machine.WindowStateMachine;
    }
}