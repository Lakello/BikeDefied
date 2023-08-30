using System;
using UnityEngine;

public abstract class EventTriggerButton : MonoBehaviour, ISubscribe
{
    public virtual event Action Action;

    public virtual void OnClick() => Action?.Invoke();
}