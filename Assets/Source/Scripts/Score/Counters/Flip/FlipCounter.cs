using System;
using UnityEngine;
using static CW.Common.CwInputManager;

namespace BikeDefied.ScoreSystem
{
    public class FlipCounter : ScoreCounter
    {
        private const int CountDirections = 4;

        private LayerMask _mask;
        private Flip _backFlip;
        private Flip _frontFlip;

        private Flip _currentFlip;
        private FlipTriggerDirection _previousFlip;
        private int _currentState;

        private bool _isBackFlip;
        private bool _isFrontFlip;

        private float _backReward;
        private float _frontReward;

        public override event Action<ScoreReward> ScoreAdd;

        public FlipCounter(float backReward, float frontReward, LayerMask mask, ScoreCounterInject inject) : base(inject)
        {
            _mask = mask;
            _backReward = backReward;
            _frontReward = frontReward;
        }

        protected struct Flip
        {
            public FlipTriggerDirection[] Directions;
        }

        protected override void Start()
        {
            _backFlip = new() 
            {
                Directions = new[]
                {
                    FlipTriggerDirection.Bottom,
                    FlipTriggerDirection.Front,
                    FlipTriggerDirection.Top,
                    FlipTriggerDirection.Back
                }
            };

            _frontFlip = new()
            {
                Directions = new[]
                {
                    FlipTriggerDirection.Bottom,
                    FlipTriggerDirection.Back,
                    FlipTriggerDirection.Top,
                    FlipTriggerDirection.Front
                }
            };

            _currentFlip = new Flip()
            {
                Directions = new FlipTriggerDirection[CountDirections]
            };

            BehaviourCoroutine = Context.StartCoroutine(Player.Behaviour(
            condition: () =>
            {
                return !IsGrounded;
            },
            action: () =>
            {
                Ray();

                if (CheckFlip(out bool direction))
                    AddScore(direction);
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
                            UpdateCurrentFlipState(trigger);
                            _previousFlip = trigger.Direction;
                        }
                    }
                }
            }
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
                        break;
                    }
                    break;
            }
        }

        private void SetBottomState(FlipTrigger trigger)
        {
            if (trigger.Direction == FlipTriggerDirection.Bottom)
            {
                _currentFlip.Directions[_currentState] = trigger.Direction;
                _currentState++;
            }
        }

        private bool CheckFlip(out bool direction)
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

        private void AddScore(bool dir)
        {
            if (dir)
            {
                Reward.Message = $"+{_frontReward}";
                Reward.Value = _frontReward;

                ScoreAdd?.Invoke(Reward);
            }
            else
            {
                Reward.Message = $"+{_backReward}";
                Reward.Value = _backReward;

                ScoreAdd?.Invoke(Reward);
            }

            _currentFlip.Directions = new FlipTriggerDirection[4];

            _currentState = 0;

            _isBackFlip = false;
            _isFrontFlip = false;
        }
    }
}