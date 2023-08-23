using Reflex.Attributes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    private IReadOnlyList<IScoreCounter> _counters;
    private int _currentScore;

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
    }

    private void Subscribe()
    {
        foreach (IScoreCounter counter in _counters)
            counter.ScoreChanged += OnScoreChanged;
    }

    private void UnSubscribe()
    {
        foreach (IScoreCounter counter in _counters)
            counter.ScoreChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(int value)
    {
        if (value < 1)
            return;

        _currentScore += value;
        _scoreText.text = _currentScore.ToString();
    }
}
