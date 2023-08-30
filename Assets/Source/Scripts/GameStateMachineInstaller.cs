﻿using Reflex.Core;
using UnityEngine;
using IJunior.StateMachine;

public class GameStateMachineInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private GameStateMachine _gameStateMachine;

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
        _gameOverState = _gameStateMachine.gameObject.GetComponent<GameOverState>();
        descriptor.AddInstance(_gameOverState, typeof(IGameOver));

        _transitionInitializer = new TransitionInitializer<GameStateMachine>(_gameStateMachine);

        _transitionInitializer.InitTransition<GameOverState>(_characterHead);
        _transitionInitializer.InitTransition<PlayState>(_startButton);
        _transitionInitializer.InitTransition<RestartState>(_restartButton);
        _transitionInitializer.InitTransition<MenuState>(_mainMenuButton);
    }
}