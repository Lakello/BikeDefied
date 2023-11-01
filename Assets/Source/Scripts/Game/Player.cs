using BikeDefied.FSM.Game;
using Reflex.Attributes;
using System;
using System.Collections;
using UnityEngine;

namespace BikeDefied.Game
{
    public class Player : MonoBehaviour
    {
        private IGameOver _game;

        public bool IsAlive { get; private set; }

        private void OnEnable()
        {
            if (_game != null)
                _game.GameOver += OnGameOver;

            IsAlive = true;
        }

        private void OnDisable()
        {
            _game.GameOver -= OnGameOver;
        }

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
            _game = inject.Over;
            _game.GameOver += OnGameOver;
        }

        private bool OnGameOver()
        {
            IsAlive = false;

            return true;
        }
    }
}