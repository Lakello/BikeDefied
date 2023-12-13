using System;
using System.Collections;
using BikeDefied.Other;
using UnityEngine;

namespace BikeDefied.AudioSystem
{
    public class AudioVolumeChanger : IDisposable, IAudioVolumeChanger
    {
        private const float MaxVolume = 1;
        private const float MinVolume = 0;

        private Context _context;
        private GameAudioHandler _gameAudioHandler;
        private AudioSource _backgroundAudio;
        private AudioSource _gameAudio;
        private Coroutine _gameAudioCoroutine;
        private Coroutine _backgroundAudioCoroutine;
        private float _volumePercent = 1f;

        public AudioVolumeChanger(
            AudioSource gameAudio,
            AudioSource backgroundAudio,
            GameAudioHandler audioHandler,
            Context context)
        {
            _gameAudioHandler = audioHandler;
            _gameAudio = gameAudio;
            _gameAudio.loop = false;
            _backgroundAudio = backgroundAudio;
            _backgroundAudio.loop = true;
            _context = context;

            _backgroundAudioCoroutine = _context.StartCoroutine(PlayBackgroundAudio());
        }

        public float VolumePercent
        {
            get => _volumePercent;
            set
            {
                _volumePercent = Mathf.Clamp01(value);
                _context.StartCoroutine(SmoothlyChangeVolume(_backgroundAudio, _volumePercent));
                _context.StartCoroutine(SmoothlyChangeVolume(_gameAudio, _volumePercent));
            }
        }
        
        public void Dispose()
        {
            if (_gameAudioCoroutine != null)
            {
                _context.StopCoroutine(_gameAudioCoroutine);
            }
            
            if (_backgroundAudioCoroutine != null)
            {
                _context.StopCoroutine(_backgroundAudioCoroutine);
            }
        }

        public void Play(AudioType audio)
        {
            if (_gameAudioCoroutine != null)
            {
                _context.StopCoroutine(_gameAudioCoroutine);
            }

            _gameAudioCoroutine = _context.StartCoroutine(PlayGameAudio(audio));
        }

        private IEnumerator PlayBackgroundAudio()
        {
            var wait = new WaitForSeconds(_gameAudioHandler.TimeBetweenChangeBackgroundAudio);

            while (Application.isPlaying)
            {
                yield return _context.StartCoroutine(SmoothlyChangeVolume(_backgroundAudio, MinVolume));
                yield return _context.StartCoroutine(
                    PlayClip(
                        _backgroundAudio,
                        _gameAudioHandler.GetRandomAudio(AudioType.Background),
                        _gameAudioHandler.MaxVolumeBackgroundAudio
                    )
                );
                
                yield return wait;
            }
        }

        private IEnumerator PlayGameAudio(AudioType audio)
        {
            if (_gameAudio.isPlaying)
            {
                yield return _context.StartCoroutine(SmoothlyChangeVolume(_gameAudio, MinVolume));
            }

            yield return _context.StartCoroutine(PlayClip(_gameAudio, _gameAudioHandler.GetRandomAudio(audio), MaxVolume));
        }

        private IEnumerator PlayClip(AudioSource source, AudioClip clip, float targetVolume)
        {
            source.clip = clip;
            source.Play();

            yield return _context.StartCoroutine(SmoothlyChangeVolume(source, targetVolume));
        }

        private IEnumerator SmoothlyChangeVolume(AudioSource source, float targetVolume)
        {
            float startVolume = _gameAudio.volume;
            float pastTime = 0;

            float normalizedTime = 0;
            float maxNormalizedTime = 1;

            while (normalizedTime <= maxNormalizedTime)
            {
                pastTime += Time.deltaTime;
                normalizedTime = pastTime / _gameAudioHandler.SmoothlyTime;
                source.volume = Mathf.Lerp(startVolume, targetVolume, normalizedTime) * _volumePercent;

                yield return null;
            }
        }
    }
}