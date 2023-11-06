using BikeDefied.FSM.Game;
using BikeDefied.UI.Animations;
using BikeDefied.Yandex.Saves;
using BikeDefied.Yandex.Saves.Data;
using Reflex.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BikeDefied.ScoreSystem
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private ObjectScaler _scoreScaler;
        [SerializeField] private ObjectScaler _flipMessageScaler;
        [SerializeField] private TMP_Text _totalScoreText;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _flipMessageText;
        [SerializeField] private float _totalScoreShowTime;

        private IReadOnlyList<IScoreCounter> _counters;
        private IEndLevelStateChangeble _endLevel;
        private float _currentScore;
        private Coroutine _totalScoreShowCoroutine;
        private Action SaveScore;

        [Inject]
        private void Inject(IReadOnlyList<IScoreCounter> counters, GameStateInject inject, ISaver saver)
        {
            _counters = counters;

            SubscribeCounters();

            SaveScore = () => saver.Set(new LevelInfo(saver.Get<CurrentLevel>().Index, (int)_currentScore));

            _endLevel = inject.EndLevel;
            _endLevel.StateChanged += OnStateChanged;
        }

        private void OnEnable()
        {
            if (_counters != null && _counters.Count >= 1)
                SubscribeCounters();

            if (_endLevel != null)
                _endLevel.StateChanged += OnStateChanged;
        }

        private void OnDisable()
        {
            if (_counters != null && _counters.Count >= 1)
                UnSubscribeCounters();

            if (_endLevel != null)
                _endLevel.StateChanged -= OnStateChanged;
        }

        private void SubscribeCounters()
        {
            foreach (IScoreCounter counter in _counters)
                counter.ScoreAdding += OnScoreAdding;
        }

        private void UnSubscribeCounters()
        {
            foreach (IScoreCounter counter in _counters)
                counter.ScoreAdding -= OnScoreAdding;
        }

        private bool OnStateChanged()
        {
            if (_totalScoreShowCoroutine != null)
                StopCoroutine(_totalScoreShowCoroutine);

            SaveScore();

            _totalScoreShowCoroutine = StartCoroutine(ShowTotalScore());

            return true;
        }

        private IEnumerator ShowTotalScore()
        {
            var targetScore = 0f;
            var previousTime = 0f;

            while (targetScore <= _currentScore)
            {
                var currentTime = (previousTime + Time.deltaTime) / _totalScoreShowTime;

                targetScore = Mathf.Lerp(targetScore, _currentScore, currentTime);

                previousTime = currentTime;

                _totalScoreText.text = targetScore.ToString("0");
                yield return null;
            }
        }

        private void OnScoreAdding(ScoreReward reward)
        {
            if (reward.Value < 1)
                return;

            _currentScore += reward.Value;

            _scoreText.text = _currentScore.ToString("0");
            _scoreScaler.Play(subAnimation: (scale) => _scoreText.alpha = scale);

            if (reward.Message != "")
            {
                _flipMessageScaler.gameObject.SetActive(true);
                _flipMessageText.text = reward.Message;
                _flipMessageScaler.Play(successAction: () => _flipMessageScaler.gameObject.SetActive(false));
            }
        }
    }
}