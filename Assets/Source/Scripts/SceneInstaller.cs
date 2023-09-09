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

    private List<IScoreCounter> _scoreCounters;

    public void InstallBindings(ContainerDescriptor descriptor)
    {
        InitBikeBehaviour(descriptor);
        InitScore(descriptor);
        InitLevelView(descriptor);

        descriptor.AddInstance(_bike);
        descriptor.AddInstance(_groundChecker);
    }

    private void InitBikeBehaviour(ContainerDescriptor descriptor)
    {
        var bikeBehaviourInject = new BikeBehaviourInject(() => (_player, _bike));

        descriptor.AddInstance(bikeBehaviourInject);
    }

    private void InitScore(ContainerDescriptor descriptor)
    {
        var scoreCounterInject = new ScoreCounterInject(() => (_player, this, _groundChecker, _bike));

        _scoreCounters = new List<IScoreCounter>()
        {
            new DistanceCounter(_distanceReward, scoreCounterInject),
            new FlipCounter(_backFlipReward, _frontFlipReward, _flipTriggerMask, scoreCounterInject)
        };

        descriptor.AddInstance(_scoreCounters, typeof(IReadOnlyList<IScoreCounter>));
    }

    private void InitLevelView(ContainerDescriptor descriptor)
    {
        descriptor.AddInstance(_selectLevelScrollView);
    }
}
