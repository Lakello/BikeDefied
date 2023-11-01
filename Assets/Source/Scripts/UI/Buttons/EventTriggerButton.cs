using System;
using UnityEngine;

namespace BikeDefied.UI.Buttons
{
    public abstract class EventTriggerButton : MonoBehaviour, ISubject
    {
        public bool IsInteractable = true;

        public virtual event Action Action;

        public virtual void OnClick()
        {
            if (!IsInteractable)
                return;

            Action?.Invoke();
        }
    }
}