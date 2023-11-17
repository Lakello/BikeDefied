using System;
using UnityEngine;

namespace BikeDefied.UI.Buttons
{
    public abstract class EventTriggerButton : MonoBehaviour, ISubject
    {
        [SerializeField] private bool _isInteractable = true;

        public bool IsInteractable { get => _isInteractable; set => _isInteractable = value; }

        public virtual event Action ActionEnded;

        public virtual void OnClick()
        {
            if (!_isInteractable)
            {
                return;
            }

            ActionEnded?.Invoke();
        }
    }
}