using System;
using UnityEngine;

namespace BikeDefied.Yandex
{
    public class FocusObserver : MonoBehaviour
    {
        private bool _isAdShow;

        public event Action<bool> FocusChanged;

        private void OnApplicationFocus(bool focus) =>
            ChangeFocus(focus, true);

        public void ChangeFocusAd(bool focus, bool isAdShow)
        {
            _isAdShow = isAdShow;
            ChangeFocus(focus, false);
        }

        private void ChangeFocus(bool focus, bool isApplication)
        {
            if (_isAdShow == true && isApplication == true)
                return;

            if (focus)
                Time.timeScale = 1.0f;
            else
                Time.timeScale = 0.0f;

            FocusChanged?.Invoke(focus);
        }
    }
}