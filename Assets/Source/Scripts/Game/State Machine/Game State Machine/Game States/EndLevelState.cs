using BikeDefied.FSM.GameWindow;
using BikeDefied.FSM.GameWindow.States;
using BikeDefied.Other;
using System;
using System.Collections;
using UnityEngine;

namespace BikeDefied.FSM.Game.States
{
    public class EndLevelState : GameState, IGameOver
    {
        private readonly Context _context;
        private Coroutine _gameOverWaitCoroutine;

        public EndLevelState(Context context, WindowStateMachine machine) : base(machine)
        {
            _context = context;
        }

        public event Func<bool> GameOver;
        public event Action LateGameOver;

        public override void Enter()
        {
            if (GameOver != null)
            {
                if (_gameOverWaitCoroutine != null)
                    _context.StopCoroutine(_gameOverWaitCoroutine);

                _gameOverWaitCoroutine = _context.StartCoroutine(GameOverWait());
            }
        }

        public override void Exit() { }

        private IEnumerator GameOverWait()
        {
            yield return new WaitUntil(GameOver.Invoke);

            Machine.EnterIn<OverWindowState>();
            LateGameOver?.Invoke();
        }
    }
}