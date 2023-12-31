using System.Collections.Generic;
using BikeDefied.Game;
using BikeDefied.Yandex.Saves;
using BikeDefied.Yandex.Saves.Data;
using Reflex.Attributes;
using UnityEngine;

namespace BikeDefied.LevelComponents
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private List<Level> _levelPrefabs;
        [SerializeField] private Vector3 _levelOffset;

        private LevelStateMachine _levelStateMachine;
        private ISaver _saver;

        [Inject]
        private void Inject(Finish finish, ISaver saver)
        {
            _saver = saver;
            _saver.SubscribeValueUpdated<CurrentLevel>(OnLevelChanged);

            Init(finish);

            _levelStateMachine.EnterIn(saver.Get<CurrentLevel>().Index);
        }

        private void OnDisable() =>
            _saver?.UnsubscribeValueUpdated<CurrentLevel>(OnLevelChanged);

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

        private void OnLevelChanged(CurrentLevel currentLevel) =>
            _levelStateMachine.EnterIn(currentLevel.Index);
    }
}