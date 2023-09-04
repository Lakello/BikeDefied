using System;
using UnityEngine;

public abstract class EventTriggerButton : MonoBehaviour, ISubject
{
    public virtual event Action Action;

    public virtual void OnClick() => Action?.Invoke();
}