using System;
using UnityEngine;
using UnityEngine.UI;

namespace BikeDefied.UI
{
    [Serializable]
    public class ControlView
    {
        [SerializeField] private Image[] _controls;

        public void Enable()
        {
            foreach (var control in _controls)
                control.gameObject.SetActive(true);
        }

        public void Disable()
        {
            foreach (var control in _controls)
                control.gameObject.SetActive(false);
        }
    }
}
