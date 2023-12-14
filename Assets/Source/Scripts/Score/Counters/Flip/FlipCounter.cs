using System;
using UnityEngine;

namespace BikeDefied.ScoreSystem
{
    public class FlipCounter : ScoreCounter
    {
        private const int CountDirections = 4;

        private readonly float _backReward;
        private readonly float _frontReward;
        private readonly LayerMask _mask;

        private Flip _currentFlip;
        private FlipTriggerDirection _previousFlip;
        private int _currentState;

        private bool _isBackFlip;
        private bool _isFrontFlip;

        public FlipCounter(float backReward, float frontReward, LayerMask mask, ScoreCounterInject inject)
            : base(inject)
        {
            _mask = mask;
            _backReward = backReward;
            _frontReward = frontReward;
        }

        public override event Action<ScoreReward> ScoreAdding;

        protected override void Start()
        {
            _currentFlip = new Flip
            {
                Directions = new FlipTriggerDirection[CountDirections],
            };

            BehaviourCoroutine = Context.StartCoroutine(Player.Behaviour(
                condition: () => !IsGrounded,
                action: () =>
                {
                    Ray();

                    if (CheckFlip(out bool direction))
                    {
                        AddScore(direction);
                    }
                }));
        }

        private void Ray()
        {
            if (!Physics.Raycast(BikeBody.position, -BikeBody.up, out RaycastHit hit, Mathf.Infinity, _mask))
            {
                return;
            }

            if (!hit.transform.TryGetComponent(out FlipTrigger trigger))
            {
                return;
            }

            if (trigger == null)
            {
                return;
            }

            if (trigger.Direction == _previousFlip)
            {
                return;
            }

            UpdateCurrentFlipState(trigger);
            _previousFlip = trigger.Direction;
        }

        private void UpdateCurrentFlipState(FlipTrigger trigger)
        {
            switch (_currentState)
            {
                case 0:
                    SetBottomState(trigger);

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
                    }

                    break;
            }
        }

        private void SetBottomState(FlipTrigger trigger)
        {
            if (trigger.Direction != FlipTriggerDirection.Bottom)
            {
                return;
            }

            _currentFlip.Directions[_currentState] = trigger.Direction;
            _currentState++;
        }

        private bool CheckFlip(out bool direction)
        {
            direction = false;

            if (_currentState == _currentFlip.Directions.Length - 1)
            {
                if (_isBackFlip)
                {
                    return _currentFlip.Directions[1] == FlipTriggerDirection.Front && _currentFlip.Directions[3] == FlipTriggerDirection.Back;
                }

                if (!_isFrontFlip)
                {
                    return true;
                }

                direction = true;

                return _currentFlip.Directions[1] == FlipTriggerDirection.Back && _currentFlip.Directions[3] == FlipTriggerDirection.Front;
            }

            return false;
        }

        private void AddScore(bool dir)
        {
            ScoreReward reward = new ScoreReward
            {
                Message = dir ? $"+{_frontReward}" : $"+{_backReward}",
                Value = dir ? _frontReward : _backReward,
            };

            ScoreAdding?.Invoke(reward);

            _currentFlip.Directions = new FlipTriggerDirection[4];

            _currentState = 0;

            _isBackFlip = false;
            _isFrontFlip = false;
        }

        private struct Flip
        {
            public FlipTriggerDirection[] Directions;
        }
    }
}