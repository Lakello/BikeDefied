using Reflex.Core;
using UnityEngine;
using IJunior.StateMachine;

public class GameStateMachineInstaller : MonoBehaviour, IInstaller
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

    public void InstallBindings(ContainerDescriptor descriptor)
    {
        var gameOverState = GameStateMachine.Instance.GetState<GameOverState>();
        var gamePlayState = GameStateMachine.Instance.GetState<PlayState>();
        var gameMenuState = GameStateMachine.Instance.GetState<MenuState>();

        var stateInject = new GameStateInject(() => (gameMenuState, gamePlayState, gameOverState));

        descriptor.AddInstance(stateInject);

        descriptor.AddInstance(_finish);

        _transitionInitializer = new TransitionInitializer<GameStateMachine>(GameStateMachine.Instance);

        _transitionInitializer.InitTransition<GameOverState>(_finish);
        _transitionInitializer.InitTransition<GameOverState>(_characterHead);
        _transitionInitializer.InitTransition<PlayState>(_startButton);
        _transitionInitializer.InitTransition<PlayState>(_restartFromGameOverButton, () => IJunior.TypedScenes.Game.Load<PlayState>());
        _transitionInitializer.InitTransition<MenuState>(_mainMenuFromGameOverButton, () => IJunior.TypedScenes.Game.Load<MenuState>());
        _transitionInitializer.InitTransition<PlayState>(_restartFromPlayButton, () => IJunior.TypedScenes.Game.Load<PlayState>());
        _transitionInitializer.InitTransition<MenuState>(_mainMenuFromPlayButton, () => IJunior.TypedScenes.Game.Load<MenuState>());
    }
}