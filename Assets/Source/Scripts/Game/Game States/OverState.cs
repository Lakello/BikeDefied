using IJunior.StateMachine;
using System;
using System.Collections;
using UnityEngine;

public class OverState : GameState, IGameOver
{
    private readonly MonoBehaviour _context;
    private Coroutine _gameOverWaitCoroutine;

    public event Func<bool> GameOver;
    public event Action LateGameOver;

    public OverState(MonoBehaviour context) =>
        _context = context;

    public override void Enter()
    {
        if (GameOver != null)
        {
            if (_gameOverWaitCoroutine != null)
                _context.StopCoroutine(_gameOverWaitCoroutine);

            _gameOverWaitCoroutine = _context.StartCoroutine(GameOverWait());
        }
    }

    public override void Exit(){}

    private IEnumerator GameOverWait()
    {
        yield return new WaitUntil(GameOver.Invoke);

        GameStateMachine.Instance.SetWindow<OverWindowState>();
        LateGameOver?.Invoke();
    }
}