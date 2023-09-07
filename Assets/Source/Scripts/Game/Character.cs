using Reflex.Attributes;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private List<FixedJoint> _joints;

    private IGameOver _game;

    [Inject]
    private void Inject(GameStateInject inject)
    {
        _game = inject.Over;
        _game.GameOver += OnGameOver;
    }

    private void OnEnable()
    {
        if (_game != null)
            _game.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        _game.GameOver -= OnGameOver;
    }

    private void OnGameOver()
    {
        foreach (var joint in _joints)
        {
            joint.connectedBody = null;
        }
    }
}
