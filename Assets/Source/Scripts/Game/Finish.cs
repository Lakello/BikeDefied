using Reflex.Attributes;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Finish : MonoBehaviour, ISubject
{
    private IAudioController _audioController;

    public event Action Action;

    private void OnEnable()
    {
        Action += OnAction;
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
    private void Inject(IAudioController audioController)
    {
        _audioController = audioController;
    }

    private void OnAction()
    {
        _audioController.Play(Audio.VictoryGameOver);
    }
}