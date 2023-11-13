using BikeDefied.FSM.Game.States;
using BikeDefied.FSM.Game;
using BikeDefied.Game.Character;
using BikeDefied.Game;
using BikeDefied.UI.Buttons;
using Reflex.Core;
using UnityEngine;
using BikeDefied.TypedScenes;
using BikeDefied.FSM;

namespace BikeDefied
{
    public class GameStateMachineInstaller : MonoBehaviour, IInstaller, ISceneLoadHandlerOnState<GameStateMachine>
    {
        [Header("Triggers for Transitions")]
        [SerializeField] private CharacterHead _characterHead;
        [SerializeField] private StartButton _startButton;
        [SerializeField] private RestartButton _restartFromGameOverButton;
        [SerializeField] private RestartButton _restartFromPlayButton;
        [SerializeField] private MainMenuButton _mainMenuFromGameOverButton;
        [SerializeField] private MainMenuButton _mainMenuFromPlayButton;
        [SerializeField] private Finish _finish;

        private TransitionInitializer<GameStateMachine> _transitionInitializer;

        private void OnEnable() =>
            _transitionInitializer?.OnEnable();

        private void OnDisable() =>
            _transitionInitializer?.OnDisable();

        public void InstallBindings(ContainerDescriptor descriptor) =>
            descriptor.AddInstance(_finish);

        public void OnSceneLoaded<TState>(GameStateMachine machine)
            where TState : State<GameStateMachine>
        {
            _transitionInitializer = new TransitionInitializer<GameStateMachine>(machine);

            _transitionInitializer.InitTransition<EndLevelState>(_finish);
            _transitionInitializer.InitTransition<EndLevelState>(_characterHead);
            _transitionInitializer.InitTransition<PlayLevelState>(_startButton);
            _transitionInitializer.InitTransition<PlayLevelState>(_restartFromGameOverButton, () => GameScene.Load<PlayLevelState>(machine));
            _transitionInitializer.InitTransition<MenuState>(_mainMenuFromGameOverButton, () => GameScene.Load<MenuState>(machine));
            _transitionInitializer.InitTransition<PlayLevelState>(_restartFromPlayButton, () => GameScene.Load<PlayLevelState>(machine));
            _transitionInitializer.InitTransition<MenuState>(_mainMenuFromPlayButton, () => GameScene.Load<MenuState>(machine));
        }
    }
}