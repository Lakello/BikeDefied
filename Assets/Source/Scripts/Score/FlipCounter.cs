using System;
using System.Diagnostics;
using UnityEngine;

public class FlipCounter : ScoreCounter
{
    public override event Action<IReward> ScoreUpdated;

    private float _previousAngle;
    private float _minCounterZone = 60f;
    private float _maxCounterZone = 300f;

    private bool _isBackFlip;
    private bool _isFrontFlip;

    private bool _isStartBackFlip;
    private bool _isStartFrontFlip;

    public FlipCounter(ScoreCounterInject inject) : base(inject) { }

    protected override void Start()
    {
        BehaviourCoroutine = Context.StartCoroutine(Player.Behaviour(
        condition: () =>
        {
            return true;
        },
        action: () =>
        {
            UnityEngine.Debug.Log(_isStartFrontFlip);

            if (_isFrontFlip || _isBackFlip)
                CallReward();

            CheckAngle();
        }));
    }

    private void CheckAngle()
    {
        var currentAngle = BikeBody.eulerAngles.x;

        if (_isStartBackFlip)
            if (currentAngle < _previousAngle)
                Reset();

        if (_isStartFrontFlip)
            if (currentAngle > _previousAngle)
                Reset();

        if (_isStartBackFlip || _isStartFrontFlip)
            WaitFlip(currentAngle);

        if (currentAngle < _minCounterZone && !_isStartFrontFlip)
            return;
        else if (currentAngle > _minCounterZone && currentAngle < _maxCounterZone)
            WaitFlip(currentAngle);
        else if (currentAngle > _maxCounterZone && !_isStartBackFlip)
            return;
    }

    private void WaitFlip(float currentAngle)
    {
        UnityEngine.Debug.Log(_isStartFrontFlip);

        if (_previousAngle == 0)
        {
            _previousAngle = currentAngle;
            return;
        }

        if (!_isStartBackFlip && !_isStartFrontFlip)
        {
            if (_previousAngle > currentAngle)
                _isStartFrontFlip = true;
            else if (_previousAngle < currentAngle)
                _isStartBackFlip = true;
        }

        if (_isStartBackFlip && currentAngle > _maxCounterZone)
            _isBackFlip = true;

        if (_isStartFrontFlip && currentAngle < _minCounterZone)
            _isFrontFlip = true;
    }

    private void CallReward()
    {
        if (_isBackFlip)
        {
            Reward.Message = "Back Flip!";
            Reward.Score = 100f;
        }

        if (_isFrontFlip)
        {
            Reward.Message = "Front Flip!";
            Reward.Score = 200f;
        }

        ScoreUpdated?.Invoke(Reward);

        Reset();
    }

    private void Reset()
    {
        _isBackFlip = false;
        _isFrontFlip = false;
        _isStartBackFlip = false;
        _isStartFrontFlip = false;
        _previousAngle = 0;
    }
}
