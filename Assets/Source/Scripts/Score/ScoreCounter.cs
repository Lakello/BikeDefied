using System;
using UnityEngine;

public abstract class ScoreCounter : IScoreCounter
{
    protected Bike Bike;
    protected Player Player;
    protected MonoBehaviour Context;
    protected Coroutine BehaviourCoroutine;
    protected IRead<RigidbodyConstraints> BikeRigidbodyConstraints;

    public abstract event Action<float> ScoreUpdated;

    public ScoreCounter(ScoreCounterInject inject)
    {
        Bike = inject.Bike;
        Player = inject.Player;
        Context = inject.Context;
        BikeRigidbodyConstraints = inject.BikeRigidbodyConstraints;

        Start();
    }

    protected abstract void Start();
}
