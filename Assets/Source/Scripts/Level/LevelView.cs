using Reflex.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelView : MonoBehaviour
{
    [SerializeField] private List<Level> _levelPrefabs;
    [SerializeField] private Button _selectButtonPrefab;

    private SelectLevelScrollView _scrollView;
    private LevelStateMachine _levelStateMachine;

    private Func<int> GetCurrentLevelIndex;
    private Action<int> SetCurrentlevelIndex;

    private void OnEnable()
    {
        if (_scrollView != null)
            _scrollView.LevelChanged += OnLevelChanged;
    }

    private void OnDisable()
    {
        _scrollView.LevelChanged -= OnLevelChanged;
    }

    [Inject]
    private void Inject(Finish finish, SelectLevelScrollView scrollView, ISaver<CurrentLevel> currentLevel)
    {
        _scrollView = scrollView;
        _scrollView.LevelChanged += OnLevelChanged;

        GetCurrentLevelIndex = () => currentLevel.Get().Index;
        SetCurrentlevelIndex = (index) => 
        {
            var level = new CurrentLevel();
            level.Index = index;
            currentLevel.Set(level);
        };

        Init(finish);

        _levelStateMachine.EnterIn(GetCurrentLevelIndex());
    }

    private void Init(Finish finish)
    {
        _levelStateMachine = new LevelStateMachine(() =>
        {
            var states = new List<LevelState>();

            foreach (var level in _levelPrefabs)
            {
                states.Add(new LevelState(level, gameObject, finish));
            }

            return states;
        });
    }

    private void OnLevelChanged(int index)
    {
        SetCurrentlevelIndex(index);
        _levelStateMachine.EnterIn(index);
    }
}
