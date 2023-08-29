using Reflex.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateMachineInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private CharacterHead _characterHead;
    [SerializeField] private StartButton _startButton;

    [Header("Game State Machine")]
    [SerializeField] private GameStateMachine _gameStateMachine;

    [Header("Window State Machine")]
    [SerializeField] private WindowStateMachine _windowStateMachine;

    private GameState _gameOverState;
    private List<Subscriber> _subscribes = new List<Subscriber>();

    protected struct Subscriber
    {
        public ISubscribe Subscribe;
        public Action Action;

        public Subscriber(ISubscribe subscribe, Action action)
        {
            Subscribe = subscribe;
            Action = action;
        }
    }
     
    private void OnEnable()
    {
        if (_subscribes != null)
            Subscribe();
    }

    private void OnDisable()
    {
        if (_subscribes != null)
            UnSubscribe();
    }

    public void InstallBindings(ContainerDescriptor descriptor)
    {
        descriptor.AddInstance(_windowStateMachine);

        InitTransition(descriptor);

        _gameOverState = _gameStateMachine.gameObject.GetComponent<GameOverState>();
        descriptor.AddInstance(_gameOverState, typeof(IGameOver));
    }

    private void InitTransition(ContainerDescriptor descriptor)
    {
        var gameOverTransition = new Transition<GameStateMachine, GameOverState>(_gameStateMachine);
        _subscribes.Add(new Subscriber(_characterHead, gameOverTransition.Transit));

        var startTransition = new Transition<GameStateMachine, PlayState>(_gameStateMachine);
        _subscribes.Add(new Subscriber(_startButton, startTransition.Transit));

        Subscribe();
    }

    private void Subscribe()
    {
        foreach (var subscriber in _subscribes)
            subscriber.Subscribe.Action += subscriber.Action;
    }

    private void UnSubscribe()
    {
        foreach (var subscriber in _subscribes)
            subscriber.Subscribe.Action -= subscriber.Action;
    }
}