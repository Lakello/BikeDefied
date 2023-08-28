using System;
using UnityEngine;

public class CharacterHead : MonoBehaviour
{
    public event Action Crash;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Ground ground))
            Crash?.Invoke();
    }
}
