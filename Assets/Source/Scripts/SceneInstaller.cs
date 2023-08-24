using Reflex.Core;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private Player _player;
    [SerializeField] private CharacterHead _characterHead;
    [SerializeField] private Bike _bike;
    [SerializeField] private Rigidbody _bikeRigidbody;

    private Game _game = new();
    private List<IScoreCounter> _scoreCounters;
    private BikeRigidbodyConstraints _bikeRigidbodyConstraints;

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

        InitBikeRigidbodyConstraints(descriptor, inputHandler);

        descriptor.AddInstance(inputHandler, typeof(IInputHandler));

        descriptor.AddInstance(_game, typeof(IGameOver));

        InitScoreCounters(descriptor);
    }

    private void InitBikeRigidbodyConstraints(ContainerDescriptor descriptor, IInputHandler inputHandler)
    {
        _bikeRigidbodyConstraints = new BikeRigidbodyConstraints(_bikeRigidbody);

        var bikeBehaviourInject = new BikeBehaviourInject();

        bikeBehaviourInject.Input = inputHandler;
        bikeBehaviourInject.Game = _game;
        bikeBehaviourInject.Player = _player;
        bikeBehaviourInject.BikeRigidbodyConstraints = _bikeRigidbodyConstraints;

        descriptor.AddInstance(bikeBehaviourInject);
    }

    private void InitScoreCounters(ContainerDescriptor descriptor)
    {
        var scoreCounterInject = new ScoreCounterInject();

        scoreCounterInject.Bike = _bike;
        scoreCounterInject.BikeRigidbodyConstraints = _bikeRigidbodyConstraints;
        scoreCounterInject.Player = _player;
        scoreCounterInject.Context = this;

        _scoreCounters = new List<IScoreCounter>()
        {
            new DistanceCounter(scoreCounterInject),
            new FlipCounter(scoreCounterInject)
        };

        descriptor.AddInstance(_scoreCounters, typeof(IReadOnlyList<IScoreCounter>));
    }
}
