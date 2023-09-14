using Reflex.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelView : MonoBehaviour
{
    [SerializeField] private List<Level> _levelPrefabs;
    [SerializeField] private Vector3 _levelOffset;

    private LevelStateMachine _levelStateMachine;
    private Action UnSubscribe;

    private void OnDisable()
    {
        UnSubscribe?.Invoke();
    }

    [Inject]
    private void Inject(Finish finish, ISaver<CurrentLevel> currentLevel)
    {
        currentLevel.ValueUpdated += OnLevelChanged;
        UnSubscribe = () => currentLevel.ValueUpdated -= OnLevelChanged;

        Init(finish);

        _levelStateMachine.EnterIn(currentLevel.Get().Index);
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
