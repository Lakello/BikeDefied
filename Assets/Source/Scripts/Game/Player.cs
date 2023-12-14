using System;
using System.Collections;
using BikeDefied.FSM.Game;
using Reflex.Attributes;
using UnityEngine;

namespace BikeDefied.Game
{
    public class Player : MonoBehaviour
    {
        private IEndLevelStateChangeble _endLevel;
        private bool _isAlive;

        [Inject]
        private void Inject(GameStateInject inject)
        {
            _endLevel = inject.EndLevel;
            _endLevel.StateChanged += OnStateChanged;
        }

        private void OnEnable()
        {
            if (_endLevel != null)
            {
                _endLevel.StateChanged += OnStateChanged;
            }

            _isAlive = true;
        }

        private void OnDisable() =>
            _endLevel.StateChanged -= OnStateChanged;

        public IEnumerator Behaviour(Func<bool> condition, Action action)
        {
            while (_isAlive)
            {
                if (!condition())
                {
                    yield return null;

                    continue;
                }

                action();

                yield return null;
            }
        }

        private bool OnStateChanged()
        {
            _isAlive = false;

            return true;
        }
    }
}