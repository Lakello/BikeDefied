using IJunior.StateMachine;
using Reflex.Attributes;
using Reflex.Core;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        var inputHandler = new PCInputHandler(input);

        descriptor.AddInstance(input);
        descriptor.AddInstance(inputHandler, typeof(IInputHandler));

        InitBikeBehaviour(descriptor);
        InitScoreCounters(descriptor);

        descriptor.AddInstance(_bike);
        descriptor.AddInstance(_selectLevelScrollView);
    }

    private void InitBikeBehaviour(ContainerDescriptor descriptor)
    {
        var bikeBehaviourInject = new BikeBehaviourInject();

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
