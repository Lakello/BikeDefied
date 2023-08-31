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

    [Header("Score")]
    [SerializeField] private float _distanceReward;
    [SerializeField] private float _backFlipReward;
    [SerializeField] private float _frontFlipReward;

    [Header("View")]
    [SerializeField] private SelectLevelScrollView _selectLevelScrollView;

    [Header("Saves")]
    [SerializeField] private Saves _saves;

    private List<IScoreCounter> _scoreCounters;

    public void InstallBindings(ContainerDescriptor descriptor)
    {
        var input = new PlayerInput();
        var inputHandler = new PCInputHandler(input);

        descriptor.AddInstance(input);
        descriptor.AddInstance(inputHandler, typeof(IInputHandler));

        InitBikeBehaviour(descriptor);
        InitScore(descriptor);
        InitLevelView(descriptor);

        descriptor.AddInstance(_bike);
        descriptor.AddInstance(_saves, typeof(IRead<CurrentLevel>));
    }

    private void InitBikeBehaviour(ContainerDescriptor descriptor)
    {
        var bikeBehaviourInject = new BikeBehaviourInject();

        bikeBehaviourInject.Player = _player;
        bikeBehaviourInject.BikeBody = _bike.GetComponent<Transform>();

        descriptor.AddInstance(bikeBehaviourInject);
    }

    private void InitScore(ContainerDescriptor descriptor)
    {
        var scoreCounterInject = new ScoreCounterInject();

        scoreCounterInject.Player = _player;
        scoreCounterInject.Context = this;
        scoreCounterInject.GroundChecker = _groundChecker;
        scoreCounterInject.BikeBody = _bike.GetComponent<Transform>();

        _scoreCounters = new List<IScoreCounter>()
        {
            new DistanceCounter(_distanceReward, scoreCounterInject),
            new FlipCounter(_backFlipReward, _frontFlipReward, _flipTriggerMask, scoreCounterInject)
        };

        descriptor.AddInstance(_scoreCounters, typeof(IReadOnlyList<IScoreCounter>));
    }

    private void InitLevelView(ContainerDescriptor descriptor)
    {
        var levelViewInject = new LevelViewInject();

        levelViewInject.SelectLevelScrollView = _selectLevelScrollView;
        levelViewInject.CurrentLevelRead = _saves;
        levelViewInject.CurrentLevelWrite = _saves;

        descriptor.AddInstance(levelViewInject);
    }
}
