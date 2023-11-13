using BikeDefied.Game;
using BikeDefied.InputSystem;
using UnityEngine;

namespace BikeDefied.BikeSystem
{
    [RequireComponent(typeof(GroundChecker))]
    public abstract class BikeBehaviour : MonoBehaviour
    {
        private Player _player;
        private IInputHandler _inputHandler;
        private Transform _bikeBody;
        private Coroutine _behaviourCoroutine;
        
        protected Player Player;
        protected IInputHandler InputHandler;
        protected Transform BikeBody;
        protected Coroutine BehaviourCoroutine;

        private GroundChecker _groundChecker;

        protected bool IsGrounded { get; private set; }
        protected bool IsBackWheelGrounded { get; private set; }

        protected abstract void Inject(BikeBehaviourInject inject);

        private void Awake()
        {
            _groundChecker = GetComponent<GroundChecker>();
            _groundChecker.GroundChanged += (value) => IsGrounded = value;
            _groundChecker.BackWheelGroundChanged += (value) => IsBackWheelGrounded = value;
        }

        private void OnDisable()
        {
            if (BehaviourCoroutine != null)
                StopCoroutine(BehaviourCoroutine);
        }

        protected void Init(BikeBehaviourInject inject)
        {
            Player = inject.Player;
            BikeBody = inject.BikeBody.transform;
        }
    }
}