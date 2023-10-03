using Reflex.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private ObjectScaler _scoreScaler;
    [SerializeField] private ObjectScaler _flipMessageScaler;
    [SerializeField] private TMP_Text _totalScoreText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _flipMessageText;
    [SerializeField] private float _totalScoreShowTime;

    private IReadOnlyList<IScoreCounter> _counters;
    private IGameOver _gameOver;
    private float _currentScore;
    private Coroutine _totalScoreShowCoroutine;
    private Action SaveScore;

    private void OnEnable()
    {
        if (_counters != null && _counters.Count >= 1)
            Subscribe();

        if (_gameOver != null)
            _gameOver.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        if (_counters != null && _counters.Count >= 1)
            UnSubscribe();

        if (_gameOver != null)
            _gameOver.GameOver -= OnGameOver;
    }

    [Inject]
    private void Inject(IReadOnlyList<IScoreCounter> counters)
    {
        _counters = counters;

        Subscribe();
    }

    [Inject]
    private void Inject(GameStateInject inject, ISaver saver)
    {
        SaveScore = () => saver.Set(new LevelInfo(saver.Get<CurrentLevel>().Index, (int)_currentScore));

        _gameOver = inject.Over;
        _gameOver.GameOver += OnGameOver;
    }

    private void Subscribe()
    {
        foreach (IScoreCounter counter in _counters)
            counter.ScoreAdd += OnScoreChanged;
    }

    private void UnSubscribe()
    {
        foreach (IScoreCounter counter in _counters)
            counter.ScoreAdd -= OnScoreChanged;
    }

    private bool OnGameOver()
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

    private void OnScoreChanged(IReward reward)
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
