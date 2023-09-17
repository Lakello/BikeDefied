using IJunior.StateMachine;
using Reflex.Core;
using UnityEngine;

public class WindowStateMachineInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private LeaderboardButton _leaderboardButton;
    [SerializeField] private MainMenuButton _backToMainMenuFromLeaderboardButton;

    private TransitionInitializer<WindowStateMachine> _transitionInitializer;

    private void OnEnable() =>
        _transitionInitializer?.OnEnable();

    private void OnDisable() =>
        _transitionInitializer?.OnDisable();

    public void InstallBindings(ContainerDescriptor descriptor)
    {
        _transitionInitializer = new TransitionInitializer<WindowStateMachine>(WindowStateMachine.Instance);

        _transitionInitializer.InitTransition<LeaderboardWindowState>(_leaderboardButton);
        _transitionInitializer.InitTransition<MenuWindowState>(_backToMainMenuFromLeaderboardButton);
    }
}