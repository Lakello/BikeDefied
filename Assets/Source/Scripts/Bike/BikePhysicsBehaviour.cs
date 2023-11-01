using BikeDefied.FSM;
using BikeDefied.FSM.Game;
using BikeDefied.FSM.Game.States;
using BikeDefied.TypedScenes;
using Reflex.Attributes;
using System.Collections;
using UnityEngine;

namespace BikeDefied.BikeSystem
{
    [RequireComponent(typeof(Rigidbody))]
    public class BikePhysicsBehaviour : MonoBehaviour, ISceneLoadHandlerOnState<GameStateMachine>
    {
        [SerializeField] private float _maxGroundMassCenter;
        [SerializeField] private float _minGroundMassCenter;
        [SerializeField] private float _maxFlyMassCenter;
        [SerializeField] private float _minFlymassCenter;
        [SerializeField] private float _maxVelocityForMaxMassCenter;

        [SerializeField] private Rigidbody _bike;
        [SerializeField] private Rigidbody _backWheel;
        [SerializeField] private Rigidbody _frontWheel;

        private IGamePlay _play;
        private GroundChecker _checker;

        private Coroutine _movePhysicsCoroutine;
        private bool _isAlive;
        private bool _isGrounded;

        private void OnEnable()
        {
            _isAlive = true;

            if (_play != null)
                _play.GamePlay += OnGamePlay;

            if (_checker != null)
                _checker.GroundChanged += OnGroundChanged;
        }

        private void OnDisable()
        {
            if (_play != null)
                _play.GamePlay -= OnGamePlay;

            if (_checker != null)
                _checker.GroundChanged -= OnGroundChanged;

            if (_movePhysicsCoroutine != null)
                StopCoroutine(_movePhysicsCoroutine);
        }

        public void OnSceneLoaded<TState>(GameStateMachine machine) where TState : State<GameStateMachine>
        {
            if (typeof(TState) == typeof(MenuState))
                _bike.isKinematic = _backWheel.isKinematic = _frontWheel.isKinematic = true;
            else if (typeof(TState) == typeof(PlayLevelState))
                _bike.isKinematic = _backWheel.isKinematic = _frontWheel.isKinematic = false;
        }

        [Inject]
        private void Inject(GameStateInject inject, GroundChecker checker)
        {
            _play = inject.Play;
            var over = inject.Over;

            _play.GamePlay += OnGamePlay;
            over.GameOver += () => _isAlive = false;

            _checker = checker;
            _checker.GroundChanged += OnGroundChanged;
        }

        private void OnGroundChanged(bool value) => _isGrounded = value;

        private void OnGamePlay()
        {
            _bike.isKinematic = _backWheel.isKinematic = _frontWheel.isKinematic = false;
            _movePhysicsCoroutine = StartCoroutine(PhysicsBehaviour());
        }

        private IEnumerator PhysicsBehaviour()
        {
            while (_isAlive)
            {
                if (_isGrounded)
                    _bike.centerOfMass = new Vector3(0, CalculateMassCenter(_maxGroundMassCenter, _minGroundMassCenter), 0);
                else
                    _bike.centerOfMass = new Vector3(0, CalculateMassCenter(_maxFlyMassCenter, _minFlymassCenter), 0);

                yield return null;
            }
        }

        private float CalculateMassCenter(float max, float min)
        {
            var current = Mathf.Abs(_bike.centerOfMass.y);
            var target = Mathf.Abs(_bike.velocity.z) > 1f ? max : min;
            var time = Mathf.Abs(_bike.velocity.z) / _maxVelocityForMaxMassCenter;

            return -Mathf.Lerp(current, target, time);
        }
    }
}
