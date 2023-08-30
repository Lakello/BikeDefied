using System;
using UnityEngine;

public class CharacterHead : MonoBehaviour, ISubscribe
{
    private bool _isInvoke;

    public event Action Action;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Ground ground))
        {
            if (!_isInvoke)
            {
                Action?.Invoke();
                _isInvoke = true;
            }
        }
    }
}
