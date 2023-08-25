using System;
using UnityEngine;

public class DistanceCounter : ScoreCounter
{
    private float _startPosition;
    private float _bestPosition;

    private float CurrentScore => _bestPosition - _startPosition;
    private float CurrentPosition => Bike.transform.position.z;

    public DistanceCounter(ScoreCounterInject inject) : base(inject) { }

    public override event Action<float> ScoreUpdated;

    protected override void Start()
    {
        _startPosition = _bestPosition = CurrentPosition;

        BehaviourCoroutine = Context.StartCoroutine(Player.Behaviour(
        condition: () =>
        {
            return IsGrounded;
        },
        action: () =>
        {
            TryUpdateScore();
        }));
    }

    private void TryUpdateScore()
    {
        if (CurrentPosition > _bestPosition)
        {
            _bestPosition = CurrentPosition;

            ScoreUpdated?.Invoke(CurrentScore);
        }
    }
}