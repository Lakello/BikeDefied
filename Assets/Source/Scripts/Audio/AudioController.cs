using System;
using System.Collections;
using UnityEngine;

public class AudioController : IDisposable, IAudioController
{
    private Context _context;
    private GameAudioHandler _gameAudioHandler;
    private AudioSource _backgroundAudio;
    private AudioSource _gameAudio;
    private Coroutine _gameAudioCoroutine;
    private Coroutine _backgroundAudioCoroutine;
    private float _volumePercent = 1f;

    public float VolumePercent
    {
        get => _volumePercent;
        set
        {
            _volumePercent = Mathf.Clamp(value, 0f, 1f);
            _context.StartCoroutine(SmoothlyChangeVolume(_backgroundAudio, _volumePercent));
            _context.StartCoroutine(SmoothlyChangeVolume(_gameAudio, _volumePercent));
        }
    }

    public AudioController(AudioSource gameAudio, AudioSource backgroundAudio,
                           GameAudioHandler audioHandler, Context context)
    {
        _gameAudioHandler = audioHandler;
        _gameAudio = gameAudio;
        _gameAudio.loop = false;
        _backgroundAudio = backgroundAudio;
        _backgroundAudio.loop = true;
        _context = context;

        _context.FocusChanged += OnFocusChanged;

        _backgroundAudioCoroutine = _context.StartCoroutine(PlayBackgroundAudio());
    }

    public void Dispose()
    {
        if (_gameAudioCoroutine != null)
            _context.StopCoroutine(_gameAudioCoroutine);

        if (_backgroundAudioCoroutine != null)
            _context.StopCoroutine(_backgroundAudioCoroutine);

        if (_context != null)
            _context.FocusChanged -= OnFocusChanged;
    }

    public void Play(Audio audio)
    {
        if (_gameAudioCoroutine != null)
            _context.StopCoroutine(_gameAudioCoroutine);

        _gameAudioCoroutine = _context.StartCoroutine(PlayClip(audio));
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

    private IEnumerator PlayBackgroundAudio()
    {
        var wait = new WaitForSeconds(_gameAudioHandler.TimeBetweenChangeBackgroundAudio);

        while (true)
        {
            yield return _context.StartCoroutine(SmoothlyChangeVolume(_backgroundAudio, 0));
            yield return _context.StartCoroutine(Control(_backgroundAudio,
                                                         _gameAudioHandler.GetRandomAudio(Audio.Background),
                                                         _gameAudioHandler.MaxVolumeBackroundAudio));
            yield return wait;
        }
    }

    private IEnumerator PlayClip(Audio audio)
    {
        if (_gameAudio.isPlaying)
            yield return _context.StartCoroutine(SmoothlyChangeVolume(_gameAudio, 0));

        yield return _context.StartCoroutine(Control(_gameAudio, _gameAudioHandler.GetRandomAudio(audio), 1));
    }

    private IEnumerator Control(AudioSource source, AudioClip clip, float targetVolume)
    {
        source.clip = clip;
        source.Play();

        yield return _context.StartCoroutine(SmoothlyChangeVolume(source, targetVolume));
    }

    private IEnumerator SmoothlyChangeVolume(AudioSource source, float targetVolume)
    {
        float startVolume = _gameAudio.volume;
        float currentTime = 0;

        float pastTime = 0;

        while (pastTime <= 1)
        {
            currentTime += Time.deltaTime;
            pastTime = currentTime / _gameAudioHandler.SmoothlyTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, pastTime) * _volumePercent;

            yield return null;
        }
    }
}