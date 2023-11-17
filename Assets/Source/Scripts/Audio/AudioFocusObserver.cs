using BikeDefied.Yandex;
using System;
using UnityEngine;

namespace BikeDefied.AudioSystem
{
    public class AudioFocusObserver : IDisposable
    {
        private readonly AudioSource _gameAudio;
        private readonly AudioSource _backgroundAudio;
        private readonly FocusObserver _focusObserver;

        public AudioFocusObserver(AudioSource gameAudio, AudioSource backgroundAudio, FocusObserver focusObserver)
        {
            _gameAudio = gameAudio;
            _backgroundAudio = backgroundAudio;
            _focusObserver = focusObserver;

            _focusObserver.FocusChanged += OnFocusChanged;
        }

        public void Dispose()
        {
            if (_focusObserver != null)
            {
                _focusObserver.FocusChanged -= OnFocusChanged;
            }
        }

        private void OnFocusChanged(bool focus)
        {
            if (focus)
            {
                _backgroundAudio.Play();
            }
            else
            {
                _backgroundAudio.Pause();
                _gameAudio.Pause();
            }
        }
    }
}