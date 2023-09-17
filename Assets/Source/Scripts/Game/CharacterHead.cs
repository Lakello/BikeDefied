using Reflex.Attributes;
using System;
using UnityEngine;

public class CharacterHead : MonoBehaviour, ISubject
{
    private IAudioController _audioController;

    public event Action Action;

    private void OnEnable()
    {
        Action += OnAction;
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
    private void Inject(IAudioController audioController)
    {
        _audioController = audioController;
    }

    private void OnAction()
    {
        _audioController.Play(Audio.LossGameOver);
    }
}
