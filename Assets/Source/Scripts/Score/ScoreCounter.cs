using System;
using UnityEngine;

public abstract class ScoreCounter : IScoreCounter, IDisposable
{
    protected Bike Bike;
    protected Player Player;
    protected MonoBehaviour Context;
    protected Coroutine BehaviourCoroutine;
    protected GroundChecker GroundChecker;

    protected bool IsGrounded { get; private set; }

    public abstract event Action<float> ScoreUpdated;

    public ScoreCounter(ScoreCounterInject inject)
    {
        Bike = inject.Bike;
        Player = inject.Player;
        Context = inject.Context;
        GroundChecker = inject.GroundChecker;
        GroundChecker.GroundChanged += OnGroundChanged;

        Start();
    }

    public void Dispose()
    {
        GroundChecker.GroundChanged -= OnGroundChanged;
    }

    protected abstract void Start();

    private void OnGroundChanged(bool value) => IsGrounded = value;
}
