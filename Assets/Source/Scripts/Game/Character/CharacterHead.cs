using BikeDefied.AudioSystem;
using BikeDefied.FSM.Game;
using BikeDefied.Other;
using Reflex.Attributes;
using System;
using UnityEngine;
using AudioType = BikeDefied.AudioSystem.AudioType;

namespace BikeDefied.Game.Character
{
    public class CharacterHead : MonoBehaviour, ISubject
    {
        private IAudioVolumeChanger _audioVolumeChanger;
        private IEndLevelStateChangeble _endLevel;

        public event Action ActionEnded;

        private void OnEnable() =>
            ActionEnded += OnActionEnded;

        private void OnDisable()
        {
            if (_endLevel != null)
                _endLevel.LateStateChanged -= OnStateChanged;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Ground ground))
            {
                ActionEnded?.Invoke();
                ActionEnded = null;
            }
        }

        [Inject]
        private void Inject(IAudioVolumeChanger audioVolumeChanger, GameStateInject states)
        {
            _audioVolumeChanger = audioVolumeChanger;
            _endLevel = states.EndLevel;
            _endLevel.LateStateChanged += OnStateChanged;
        }

        private void OnActionEnded() =>
            _audioVolumeChanger.Play(AudioType.LossGameOver);

        private void OnStateChanged() =>
            ActionEnded = null;
    }
}