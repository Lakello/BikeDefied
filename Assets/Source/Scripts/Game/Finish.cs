using System;
using BikeDefied.AudioSystem;
using BikeDefied.BikeSystem;
using BikeDefied.FSM.Game;
using Reflex.Attributes;
using UnityEngine;
using AudioType = BikeDefied.AudioSystem.AudioType;

namespace BikeDefied.Game
{
    public class Finish : MonoBehaviour, ISubject
    {
        private IAudioVolumeChanger _audioVolumeChanger;
        private IEndLevelStateChangeble _endLevel;

        public event Action ActionEnded;
        
        [Inject]
        private void Inject(IAudioVolumeChanger audioVolumeChanger, GameStateInject states)
        {
            _audioVolumeChanger = audioVolumeChanger;
            _endLevel = states.EndLevel;
            _endLevel.LateStateChanged += OnStateChanged;
        }

        private void OnEnable() =>
            ActionEnded += OnActionEnded;

        private void OnDisable()
        {
            if (_endLevel != null)
            {
                _endLevel.LateStateChanged -= OnStateChanged;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Bike bike))
            {
                ActionEnded?.Invoke();
                ActionEnded = null;
            }
        }

        public void OnPointEnabled(Vector3 position) =>
            transform.position = position;

        private void OnActionEnded() =>
            _audioVolumeChanger.Play(AudioType.VictoryGameOver);

        private void OnStateChanged() =>
            ActionEnded = null;
    }
}