﻿using BikeDefied.AudioSystem;
using BikeDefied.BikeSystem;
using BikeDefied.FSM.Game;
using Reflex.Attributes;
using System;
using UnityEngine;
using AudioType = BikeDefied.AudioSystem.AudioType;

namespace BikeDefied.Game
{
    public class Finish : MonoBehaviour, ISubject
    {
        private IAudioController _audioController;
        private IEndLevelStateChangeble _endLevel;

        [Inject]
        private void Inject(IAudioController audioController, GameStateInject states)
        {
            _audioController = audioController;
            _endLevel = states.EndLevel;
            _endLevel.LateStateChanged += OnStateChanged;
        }

        public event Action ActionEnded;

        private void OnEnable() =>
            ActionEnded += OnActionEnded;

        private void OnDisable()
        {
            if (_endLevel != null)
                _endLevel.LateStateChanged -= OnStateChanged;
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
            _audioController.Play(AudioType.VictoryGameOver);

        private void OnStateChanged() =>
            ActionEnded = null;
    }
}