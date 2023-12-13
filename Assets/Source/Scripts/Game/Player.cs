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

        public bool IsAlive { get; private set; }

        private void OnEnable()
        {
            if (_endLevel != null)
            {
                _endLevel.StateChanged += OnStateChanged;
            }

            IsAlive = true;
        }

        private void OnDisable() =>
            _endLevel.StateChanged -= OnStateChanged;

        public IEnumerator Behaviour(Func<bool> condition, Action action)
        {
            while (IsAlive)
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

        [Inject]
        private void Inject(GameStateInject inject)
        {
            _endLevel = inject.EndLevel;
            _endLevel.StateChanged += OnStateChanged;
        }

        private bool OnStateChanged()
        {
            IsAlive = false;

            return true;
        }
    }
}