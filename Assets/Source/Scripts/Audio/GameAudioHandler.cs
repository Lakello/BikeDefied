using System.Collections.Generic;
using UnityEngine;

namespace BikeDefied.AudioSystem
{
    public class GameAudioHandler : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> _backgroundMusics;
        [SerializeField] private List<AudioClip> _levelPlayAudios;
        [SerializeField] private List<AudioClip> _lossGameOverAudios;
        [SerializeField] private List<AudioClip> _victoryGameOverAudios;
        [SerializeField, Range(0f, 1f)] private float _smoothlyTime = 0.2f;
        [SerializeField, Range(30f, 300f)] private float _timeBetweenChangeBackgroundAudio = 60f;
        [SerializeField, Range(0f, 1f)] private float _maxVolumeBackgroundAudio = 0.8f;

        public float SmoothlyTime => _smoothlyTime;
        public float TimeBetweenChangeBackgroundAudio => _timeBetweenChangeBackgroundAudio;
        public float MaxVolumeBackroundAudio => _maxVolumeBackgroundAudio;

        private Dictionary<AudioType, List<AudioClip>> _clips;

        public void Init()
        {
            _clips = new Dictionary<AudioType, List<AudioClip>>()
            {
                [AudioType.Background] = _backgroundMusics,
                [AudioType.LevelPlay] = _levelPlayAudios,
                [AudioType.LossGameOver] = _lossGameOverAudios,
                [AudioType.VictoryGameOver] = _victoryGameOverAudios,
            };
        }

        public AudioClip GetRandomAudio(AudioType audio)
        {
            if (_clips.TryGetValue(audio, out List<AudioClip> clips))
            {
                return clips[Random.Range(0, clips.Count)];
            }

            return null;
        }
    }
}