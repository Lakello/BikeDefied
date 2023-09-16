using System;
using UnityEngine;

public abstract class EventTriggerButton : MonoBehaviour, ISubject
{
    public virtual event Action Action;

    public virtual void OnClick()
    {
        Debug.Log($"CLICK");
        Action?.Invoke();
    }
}