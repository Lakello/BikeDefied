using Reflex.Core;
using UnityEngine;
using IJunior.StateMachine;

public class GameStateMachineInstaller : MonoBehaviour, IInstaller
{
    [Header("Triggers for Transitions")]
    [SerializeField] private CharacterHead _characterHead;
    [SerializeField] private StartButton _startButton;
    [SerializeField] private RestartButton _restartButton;
    [SerializeField] private MainMenuButton _mainMenuButton;

    private TransitionInitializer<GameStateMachine> _transitionInitializer;
    private GameOverState _gameOverState;

    private void OnEnable() => _transitionInitializer?.OnEnable();
    private void OnDisable() => _transitionInitializer?.OnDisable();

    public void InstallBindings(ContainerDescriptor descriptor)
    {
        _gameOverState = GameStateMachine.Instance.GetState<GameOverState>();
        descriptor.AddInstance(_gameOverState, typeof(IGameOver));

        _transitionInitializer = new TransitionInitializer<GameStateMachine>(GameStateMachine.Instance);

        _transitionInitializer.InitTransition<GameOverState>(_characterHead);
        _transitionInitializer.InitTransition<PlayState>(_startButton);
        _transitionInitializer.InitTransition<MenuState>(_restartButton, () => IJunior.TypedScenes.Game.Load<PlayState>());
        _transitionInitializer.InitTransition<MenuState>(_mainMenuButton, () => IJunior.TypedScenes.Game.Load<MenuState>());
    }
}