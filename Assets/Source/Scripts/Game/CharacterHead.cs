using Reflex.Attributes;
using System;
using UnityEngine;

public class CharacterHead : MonoBehaviour, ISubject
{
    private IAudioController _audioController;
    private IGameOver _over;

    public event Action Action;

    private void OnEnable() =>
        Action += OnAction;

    private void OnDisable()
    {
        if (_over != null)
            _over.LateGameOver -= OnGameOver;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Ground ground))
        {
            Action?.Invoke();
            Action = null;
        }
    }

    [Inject]
    private void Inject(IAudioController audioController, GameStateInject states)
    {
        _audioController = audioController;
        _over = states.Over;
        _over.LateGameOver += OnGameOver;
    }

    private void OnAction() =>
        _audioController.Play(Audio.LossGameOver);

    private void OnGameOver() =>
        Action = null;
}
