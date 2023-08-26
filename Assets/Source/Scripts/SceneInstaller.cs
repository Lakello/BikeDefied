using Reflex.Core;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private Player _player;
    [SerializeField] private CharacterHead _characterHead;
    [SerializeField] private Bike _bike;
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private LayerMask _flipTriggerMask;

    private Game _game = new();
    private List<IScoreCounter> _scoreCounters;

    private void OnEnable()
    {
        _characterHead.Crash += _game.OnCrash;
    }

    private void OnDisable()
    {
        _characterHead.Crash -= _game.OnCrash;
    }

    public void InstallBindings(ContainerDescriptor descriptor)
    {
        var input = new PlayerInput();
        input.Enable();

        var inputHandler = new PCInputHandler(input);

        descriptor.AddInstance(_bike);

        InitBikeRigidbodyConstraints(descriptor, inputHandler);

        descriptor.AddInstance(inputHandler, typeof(IInputHandler));

        descriptor.AddInstance(_game, typeof(IGameOver));

        InitScoreCounters(descriptor);
    }

    private void InitBikeRigidbodyConstraints(ContainerDescriptor descriptor, IInputHandler inputHandler)
    {
        var bikeBehaviourInject = new BikeBehaviourInject();

        bikeBehaviourInject.Input = inputHandler;
        bikeBehaviourInject.Game = _game;
        bikeBehaviourInject.Player = _player;
        bikeBehaviourInject.BikeBody = _bike.GetComponent<Transform>();

        descriptor.AddInstance(bikeBehaviourInject);
    }

    private void InitScoreCounters(ContainerDescriptor descriptor)
    {
        var scoreCounterInject = new ScoreCounterInject();

        scoreCounterInject.Bike = _bike;
        scoreCounterInject.Player = _player;
        scoreCounterInject.Context = this;
        scoreCounterInject.GroundChecker = _groundChecker;
        scoreCounterInject.BikeBody = _bike.GetComponent<Transform>();

        _scoreCounters = new List<IScoreCounter>()
        {
            new DistanceCounter(scoreCounterInject),
            new FlipCounter(_flipTriggerMask, scoreCounterInject)
        };

        descriptor.AddInstance(_scoreCounters, typeof(IReadOnlyList<IScoreCounter>));
    }
}
