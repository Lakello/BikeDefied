using BikeDefied.Game;
using UnityEngine;

namespace BikeDefied.LevelComponents
{
    public class LevelState
    {
        private Level _level;
        private FinishPoint _finishPoint;
        private StartPoint _startPoint;
        private Finish _finish;

        public LevelState(Level levelPrefab, GameObject parent, Finish finish, Vector3 offset)
        {
            _level = Object.Instantiate(levelPrefab, parent.transform);
            _level.gameObject.SetActive(false);
            _finish = finish;

            _finishPoint = _level.GetComponentInChildren<FinishPoint>();
            _startPoint = _level.GetComponentInChildren<StartPoint>();

            _level.transform.position = -_startPoint.transform.position + -offset;
        }

        public void Enter()
        {
            _level.gameObject.SetActive(true);
            _finish.OnPointEnabled(_finishPoint.transform.position);
        }

        public void Exit() =>
            _level.gameObject.SetActive(false);
    }
}