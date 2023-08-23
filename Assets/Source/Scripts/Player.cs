using Reflex.Attributes;
using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private IGameOver _game;

    public bool IsAlive { get; private set; }

    private void OnEnable()
    {
        if (_game != null)
            _game.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        _game.GameOver -= OnGameOver;
    }

    public IEnumerator Behaviour(Func<bool> condition, Action action)
    {
        while (IsAlive)
        {
            if (condition())
            {
                yield return null;
                continue;
            }

            action();

            yield return null;
        }
    }

    [Inject]
    private void Inject(IGameOver game)
    {
        _game = game;
        _game.GameOver += OnGameOver;
    }

    private void OnGameOver()
    {
        IsAlive = false;
    }
}