using IJunior.StateMachine;
using IJunior.TypedScenes;
using Reflex.Attributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelView : MonoBehaviour
{
    [SerializeField] private List<Level> _levelPrefabs;
    [SerializeField] private Button _selectButtonPrefab;

    private SelectLevelScrollView _scrollView;
    private LevelStateMachine _stateMachine;

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

        _stateMachine.EnterIn(0);
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
    private void Inject(SelectLevelScrollView scrollView)
    {
        _scrollView = scrollView;
        _scrollView.LevelChanged += OnLevelChanged;
    }

    private void OnLevelChanged(int index)
    {
        _stateMachine.EnterIn(index);
    }
}
