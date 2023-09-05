using System;
using UnityEngine;

public class Finish : MonoBehaviour, ISubject
{
    public event Action Action;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Bike bike))
            Action?.Invoke();
    }

    public void OnPointEnabled(Vector3 position)
    {
        transform.position = position;
    }
}