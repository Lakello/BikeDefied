using IJunior.StateMachine;
using IJunior.TypedScenes;
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
    private LevelStateMachine _stateMachine;

    private Func<int> GetCurrentLevelIndex;
    private Action<int> SetCurrentlevelIndex;

    private void Awake()
    {
        _stateMachine = new LevelStateMachine(() =>
        {
            var states = new List<LevelState>();

            foreach (var level in _levelPrefabs)
            {
                states.Add(new LevelState(level, gameObject));
            }

            return states;
        });
    }

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
    private void Inject(LevelViewInject inject)
    {
        _scrollView = inject.SelectLevelScrollView;
        _scrollView.LevelChanged += OnLevelChanged;

        GetCurrentLevelIndex = () => inject.CurrentLevelRead.Read().Index;
        SetCurrentlevelIndex = (index) => 
        {
            var level = new CurrentLevel();
            level.Index = index;
            inject.CurrentLevelWrite.Write(level);
        };

        _stateMachine.EnterIn(GetCurrentLevelIndex());
    }

    private void OnLevelChanged(int index)
    {
        SetCurrentlevelIndex(index);
        _stateMachine.EnterIn(index);
    }
}
