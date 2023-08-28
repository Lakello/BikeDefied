using System;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class FlipCounter : ScoreCounter
{
    private LayerMask _mask;
    private Flip _backFlip;
    private Flip _frontFlip;

    private Flip _currentFlip;
    private FlipTriggerDirection _previousFlip;
    private int _currentState;

    private bool _isBackFlip;
    private bool _isFrontFlip;

    public override event Action<IReward> ScoreUpdated;

    public FlipCounter(LayerMask mask, ScoreCounterInject inject) : base(inject) { _mask = mask; }

    protected struct Flip
    {
        public FlipTriggerDirection[] Directions;
    }

    protected override void Start()
    {
        _backFlip = new Flip();
        _backFlip.Directions = new FlipTriggerDirection[4]
        {
            FlipTriggerDirection.Bottom,
            FlipTriggerDirection.Front,
            FlipTriggerDirection.Top,
            FlipTriggerDirection.Back
        };

        _frontFlip = new Flip();
        _frontFlip.Directions = new FlipTriggerDirection[4]
        {
            FlipTriggerDirection.Bottom,
            FlipTriggerDirection.Back,
            FlipTriggerDirection.Top,
            FlipTriggerDirection.Front
        };

        _currentFlip = new Flip();
        _currentFlip.Directions = new FlipTriggerDirection[4];

        BehaviourCoroutine = Context.StartCoroutine(Player.Behaviour(
        condition: () =>
        {
            return !IsGrounded;
        },
        action: () =>
        {
            Ray();

            if (Check(out bool direction))
                Rew(direction);
        }));
    }

    private void Ray()
    {
        if (Physics.Raycast(BikeBody.position, -BikeBody.up, out RaycastHit hit, Mathf.Infinity, _mask))
        {
            if (hit.transform.TryGetComponent(out FlipTrigger trigger))
            {
                if (trigger != null)
                {
                    if (trigger.Direction != _previousFlip)
                    {
                        UpdateCurrentFlip(trigger);
                        _previousFlip = trigger.Direction;
                    }
                }
            }
        }
    }

    private void UpdateCurrentFlip(FlipTrigger trigger)
    {
        switch (_currentState)
        {
            case 0:
                if (trigger.Direction == FlipTriggerDirection.Bottom)
                {
                    _currentFlip.Directions[_currentState] = trigger.Direction;
                    _currentState++;
                }
                break;
            case 1:
                if (trigger.Direction == FlipTriggerDirection.Front)
                {
                    _isBackFlip = true;
                    _isFrontFlip = false;
                    _currentFlip.Directions[_currentState] = trigger.Direction;
                    _currentState++;
                    break;
                }

                if (trigger.Direction == FlipTriggerDirection.Back)
                {
                    _isFrontFlip = true;
                    _isBackFlip = false;
                    _currentFlip.Directions[_currentState] = trigger.Direction;
                    _currentState++;
                    break;
                }
             
                _currentState--;
                
                break;
            case 2:
                if (trigger.Direction == FlipTriggerDirection.Top)
                {
                    _currentFlip.Directions[_currentState] = trigger.Direction;
                    _currentState++;
                    break;
                }

                _currentState--;
                break;
            case 3:
                if (_isBackFlip)
                {
                    if (trigger.Direction == FlipTriggerDirection.Back)
                    {
                        _currentFlip.Directions[_currentState] = trigger.Direction;
                        break;
                    }

                    _currentState--;
                    break;
                }

                if (_isFrontFlip)
                {
                    if (trigger.Direction == FlipTriggerDirection.Front)
                    {
                        _currentFlip.Directions[_currentState] = trigger.Direction;
                        break;
                    }
                    
                    _currentState--;
                    break;
                }
                break;
        }
    }

    private bool Check(out bool direction)
    {
        direction = false;

        if (_currentState == _currentFlip.Directions.Length - 1)
        {
            if (_isBackFlip)
            {
                if (_currentFlip.Directions[1] == FlipTriggerDirection.Front && _currentFlip.Directions[3] == FlipTriggerDirection.Back)
                    return true;

                return false;
            }

            if (_isFrontFlip)
            {
                direction = true;

                if (_currentFlip.Directions[1] == FlipTriggerDirection.Back && _currentFlip.Directions[3] == FlipTriggerDirection.Front)
                    return true;

                return false;
            }
        }
        else
            return false;

        return true;
    }

    private void Rew(bool dir)
    {
        if (dir)
        {
            Reward.Message = "Front Flip!";
            Reward.Score = 100;

            ScoreUpdated?.Invoke(Reward);
        }
        else
        {
            Reward.Message = "Back Flip!";
            Reward.Score = 100;

            ScoreUpdated?.Invoke(Reward);
        }

        _currentFlip.Directions = new FlipTriggerDirection[4];

        _currentState = 0;

        _isBackFlip = false;
        _isFrontFlip = false;
    }
}
