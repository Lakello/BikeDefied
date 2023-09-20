using Reflex.Attributes;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Finish : MonoBehaviour, ISubject
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Bike bike))
        {
            Action?.Invoke();
            Action = null;
        }
    }

    public void OnPointEnabled(Vector3 position)
    {
        transform.position = position;
    }

    [Inject]
    private void Inject(IAudioController audioController, GameStateInject states)
    {
        _audioController = audioController;
        _over = states.Over;
        _over.LateGameOver += OnGameOver;
    }

    private void OnAction()
    {
        _audioController.Play(Audio.VictoryGameOver);
    }

    private void OnGameOver() =>
        Action = null;
}