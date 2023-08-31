using System;
using UnityEngine;

public abstract class ScoreCounter : IScoreCounter, IDisposable
{
    protected Player Player;
    protected MonoBehaviour Context;
    protected Coroutine BehaviourCoroutine;
    protected Transform BikeBody;
    protected ScoreReward Reward;
    
    private GroundChecker _groundChecker;

    public abstract string Name { get; }

    protected bool IsGrounded { get; private set; }

    public abstract event Action<IReward> ScoreAdd;

    public ScoreCounter(ScoreCounterInject inject)
    {
        Player = inject.Player;
        Context = inject.Context;
        BikeBody = inject.BikeBody;
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
