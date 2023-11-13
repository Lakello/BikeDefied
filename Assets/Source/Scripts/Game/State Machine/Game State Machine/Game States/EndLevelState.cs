using BikeDefied.FSM.GameWindow;
using BikeDefied.FSM.GameWindow.States;
using BikeDefied.Other;
using System;
using System.Collections;
using UnityEngine;

namespace BikeDefied.FSM.Game.States
{
    public class EndLevelState : GameState, IEndLevelStateChangeble
    {
        private readonly Context _context;
        private Coroutine _gameOverWaitCoroutine;

        public EndLevelState(Context context, WindowStateMachine machine)
            : base(machine) => 
            _context = context;

        public event Func<bool> StateChanged;
        public event Action LateStateChanged;

        public override void Enter()
        {
            if (StateChanged != null)
            {
                if (_gameOverWaitCoroutine != null)
                    _context.StopCoroutine(_gameOverWaitCoroutine);

                _gameOverWaitCoroutine = _context.StartCoroutine(GameOverWait());
            }
        }

        public override void Exit() { }

        private IEnumerator GameOverWait()
        {
            yield return new WaitUntil(StateChanged.Invoke);

            WindowStateMachine.EnterIn<OverWindowState>();
            LateStateChanged?.Invoke();
        }
    }
}