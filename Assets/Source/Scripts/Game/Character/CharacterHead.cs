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
        private IAudioController _audioController;
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
        private void Inject(IAudioController audioController, GameStateInject states)
        {
            _audioController = audioController;
            _endLevel = states.EndLevel;
            _endLevel.LateStateChanged += OnStateChanged;
        }

        private void OnActionEnded() =>
            _audioController.Play(AudioType.LossGameOver);

        private void OnStateChanged() =>
            ActionEnded = null;
    }
}