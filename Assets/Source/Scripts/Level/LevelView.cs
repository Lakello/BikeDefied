using Reflex.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelView : MonoBehaviour
{
    [SerializeField] private List<Level> _levelPrefabs;
    [SerializeField] private Vector3 _levelOffset;

    private LevelStateMachine _levelStateMachine;
    private Action<CurrentLevel> _currentLevelUpdated;

    private void OnDisable()
    {
        if (_currentLevelUpdated != null)
            _currentLevelUpdated -= OnLevelChanged;
    }

    [Inject]
    private void Inject(Finish finish, ISaver saver)
    {
        _currentLevelUpdated = saver.ValueUpdated<CurrentLevel>();
        _currentLevelUpdated += OnLevelChanged;

        Init(finish);

        _levelStateMachine.EnterIn(saver.Get<CurrentLevel>().Index);
    }

    private void Init(Finish finish)
    {
        _levelStateMachine = new LevelStateMachine(() =>
        {
            var states = new List<LevelState>();

            foreach (var level in _levelPrefabs)
            {
                states.Add(new LevelState(level, gameObject, finish, _levelOffset));
            }

            return states;
        });
    }

    private void OnLevelChanged(CurrentLevel currentLevel)
    {
        _levelStateMachine.EnterIn(currentLevel.Index);
    }
}
