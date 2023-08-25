using Reflex.Attributes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _messageText;

    private IReadOnlyList<IScoreCounter> _counters;
    private float _currentScore;

    private void OnEnable()
    {
        if (_counters != null && _counters.Count >= 1)
            Subscribe();
    }

    private void OnDisable()
    {
        if (_counters != null && _counters.Count >= 1)
            UnSubscribe();
    }

    [Inject]
    private void Inject(IReadOnlyList<IScoreCounter> counters)
    {
        _counters = counters;

        Subscribe();
    }

    private void Subscribe()
    {
        foreach (IScoreCounter counter in _counters)
            counter.ScoreUpdated += OnScoreChanged;
    }

    private void UnSubscribe()
    {
        foreach (IScoreCounter counter in _counters)
            counter.ScoreUpdated -= OnScoreChanged;
    }

    private void OnScoreChanged(IReward reward)
    {
        if (reward.Score < 1)
            return;

        _currentScore += reward.Score;

        _scoreText.text = _currentScore.ToString("0.0");

        if (reward.Message != "")
            _messageText.text = reward.Message;
    }
}
