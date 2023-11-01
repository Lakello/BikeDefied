using BikeDefied.BikeSystem;
using BikeDefied.Game;
using System;
using UnityEngine;

namespace BikeDefied.ScoreSystem
{
    public abstract class ScoreCounter : IScoreCounter, IDisposable
    {
        protected Player Player;
        protected MonoBehaviour Context;
        protected Coroutine BehaviourCoroutine;
        protected Transform BikeBody;
        protected ScoreReward Reward;

        private GroundChecker _groundChecker;

        protected bool IsGrounded { get; private set; }

        public abstract event Action<ScoreReward> ScoreAdd;

        public ScoreCounter(ScoreCounterInject inject)
        {
            Player = inject.Player;
            Context = inject.Context;
            BikeBody = inject.BikeBody.transform;
            _groundChecker = inject.GroundChecker;
            _groundChecker.GroundChanged += OnGroundChanged;

            Start();
        }

        public void Dispose()
        {
            _groundChecker.GroundChanged -= OnGroundChanged;
        }

        protected abstract void Start();

        private void OnGroundChanged(bool value) => IsGrounded = value;
    }
}