using Reflex.Core;
using System.Collections.Generic;
using UnityEngine;

public class SceneInstaller : MonoBehaviour, IInstaller
{
    [Header("Bike")]
    [SerializeField] private Player _player;
    [SerializeField] private Bike _bike;
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private LayerMask _flipTriggerMask;

    [Header("View")]
    [SerializeField] private SelectLevelScrollView _selectLevelScrollView;

    private List<IScoreCounter> _scoreCounters;

    public void InstallBindings(ContainerDescriptor descriptor)
    {
        var input = new PlayerInput();
        descriptor.AddInstance(input, typeof(PlayerInput));

        var inputHandler = new PCInputHandler(input);

        InitBikeBehaviour(descriptor, inputHandler);
        InitScoreCounters(descriptor);

        descriptor.AddInstance(_bike);
        descriptor.AddInstance(_selectLevelScrollView);
    }

    private void InitBikeBehaviour(ContainerDescriptor descriptor, IInputHandler inputHandler)
    {
        var bikeBehaviourInject = new BikeBehaviourInject();

        bikeBehaviourInject.Input = inputHandler;
        bikeBehaviourInject.Player = _player;
        bikeBehaviourInject.BikeBody = _bike.GetComponent<Transform>();

        descriptor.AddInstance(bikeBehaviourInject);
    }

    private void InitScoreCounters(ContainerDescriptor descriptor)
    {
        var scoreCounterInject = new ScoreCounterInject();

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
