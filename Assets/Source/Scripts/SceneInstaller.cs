using System.Collections.Generic;
using BikeDefied.BikeSystem;
using BikeDefied.Game;
using BikeDefied.InputSystem;
using BikeDefied.ScoreSystem;
using Reflex.Core;
using UnityEngine;

namespace BikeDefied
{
    [RequireComponent(typeof(InputHandlerInitializer))]
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

        private InputHandlerInitializer _inputHandler;
        private List<IScoreCounter> _scoreCounters;

        public void InstallBindings(ContainerDescriptor descriptor)
        {
            InitBikeBehaviour(descriptor);
            InitScore(descriptor);

            descriptor.AddInstance(_bike);
            descriptor.AddInstance(_groundChecker);
        }

        private void InitBikeBehaviour(ContainerDescriptor descriptor)
        {
            IInputHandler inputHandler = GetComponent<InputHandlerInitializer>().Init();
            
            BikeBehaviourInject bikeBehaviourInject = new BikeBehaviourInject(() => (_player, _bike, inputHandler));

            descriptor.AddInstance(bikeBehaviourInject);
        }

        private void InitScore(ContainerDescriptor descriptor)
        {
            var scoreCounterInject = new ScoreCounterInject(() => (_player, this, _groundChecker, _bike));

            _scoreCounters = new List<IScoreCounter>()
            {
                new DistanceCounter(_distanceReward, scoreCounterInject),
                new FlipCounter(_backFlipReward, _frontFlipReward, _flipTriggerMask, scoreCounterInject),
            };

            descriptor.AddInstance(_scoreCounters, typeof(IReadOnlyList<IScoreCounter>));
        }
    }
}