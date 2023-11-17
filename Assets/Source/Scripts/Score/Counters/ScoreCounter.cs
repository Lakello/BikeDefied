using BikeDefied.BikeSystem;
using BikeDefied.Game;
using System;
using UnityEngine;

namespace BikeDefied.ScoreSystem
{
    public abstract class ScoreCounter : IScoreCounter, IDisposable
    {
        private readonly GroundChecker _groundChecker;
        
        private Coroutine _behaviourCoroutine;

        protected ScoreCounter(ScoreCounterInject inject)
        {
            Player = inject.Player;
            Context = inject.Context;
            BikeBody = inject.BikeBody.transform;
            _groundChecker = inject.GroundChecker;
            _groundChecker.GroundChanged += (value) => IsGrounded = value;

            Start();
        }
        
        protected Player Player { get; }
        
        protected MonoBehaviour Context { get; }
        
        protected Coroutine BehaviourCoroutine { set => _behaviourCoroutine ??= value; }
        
        protected Transform BikeBody { get; }
        
        protected bool IsGrounded { get; private set; }

        public abstract event Action<ScoreReward> ScoreAdding;
        
        public void Dispose()
        {
            if (_behaviourCoroutine != null)
                Context.StopCoroutine(_behaviourCoroutine);
        }

        protected abstract void Start();
    }
}