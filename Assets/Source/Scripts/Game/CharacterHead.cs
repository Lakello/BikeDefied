using System;
using UnityEngine;

public class CharacterHead : MonoBehaviour, ISubject
{
    public event Action Action;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Ground ground))
        {
            Action?.Invoke();
        }
    }
}
