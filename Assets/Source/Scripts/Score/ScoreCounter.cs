using System;
using UnityEngine;

public abstract class ScoreCounter : IScoreCounter, IDisposable
{
    protected Bike Bike;
    protected Player Player;
    protected MonoBehaviour Context;
    protected Coroutine BehaviourCoroutine;
    protected bool IsGrounded { get; private set; }

    private GroundChecker _groundChecker;

    public abstract event Action<float> ScoreUpdated;

    public ScoreCounter(ScoreCounterInject inject)
    {
        Bike = inject.Bike;
        Player = inject.Player;
        Context = inject.Context;
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
