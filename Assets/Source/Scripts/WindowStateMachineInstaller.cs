using IJunior.StateMachine;
using Reflex.Core;
using UnityEngine;

public class WindowStateMachineInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private LeaderboardButton _leaderboardButton;
    [SerializeField] private SettingsButton _settingsButton;
    [SerializeField] private MainMenuButton _backToMainMenuFromLeaderboardButton;
    [SerializeField] private MainMenuButton _backToMainMenuFromSettingsButton;

    private TransitionInitializer<WindowStateMachine> _transitionInitializer;

    private void OnEnable() =>
        _transitionInitializer?.OnEnable();

    private void OnDisable() =>
        _transitionInitializer?.OnDisable();

    public void InstallBindings(ContainerDescriptor descriptor)
    {
        _transitionInitializer = new TransitionInitializer<WindowStateMachine>(WindowStateMachine.Instance);

        _transitionInitializer.InitTransition<LeaderboardWindowState>(_leaderboardButton);
        _transitionInitializer.InitTransition<SettingsWindowState>(_settingsButton);
        _transitionInitializer.InitTransition<MenuWindowState>(_backToMainMenuFromSettingsButton);
        _transitionInitializer.InitTransition<MenuWindowState>(_backToMainMenuFromLeaderboardButton);
    }
}