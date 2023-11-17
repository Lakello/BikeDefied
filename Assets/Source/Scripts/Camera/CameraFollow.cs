using BikeDefied.FSM.Game;
using Reflex.Attributes;
using UnityEngine;

namespace BikeDefied.CameraSystem
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _lookAt;
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _offset;

        private IEndLevelStateChangeble _endLevel;

        [Inject]
        private void Inject(GameStateInject inject)
        {
            _endLevel = inject.EndLevel;
            _endLevel.StateChanged += OnStateChanged;
        }

        private void OnEnable()
        {
            if (_endLevel != null)
            {
                _endLevel.StateChanged += OnStateChanged;
            }
        }

        private void OnDisable()
        {
            _endLevel.StateChanged -= OnStateChanged;
        }

        private void Update()
        {
            transform.position = _target.position + _offset;
        }

        private bool OnStateChanged()
        {
            enabled = false;
            _lookAt.parent = null;

            return true;
        }
    }
}