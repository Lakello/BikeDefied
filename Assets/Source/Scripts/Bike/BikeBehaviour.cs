using BikeDefied.Game;
using BikeDefied.InputSystem;
using UnityEngine;

namespace BikeDefied.BikeSystem
{
    [RequireComponent(typeof(GroundChecker))]
    public abstract class BikeBehaviour : MonoBehaviour
    {
        protected Player Player;
        protected IInputHandler InputHandler;
        protected Transform BikeBody;
        protected Coroutine BehaviourCoroutine;

        private GroundChecker _groundChecker;

        protected bool IsGrounded { get; private set; }
        protected bool IsBackWheelGrounded { get; private set; }

        private void Awake()
        {
            _groundChecker = GetComponent<GroundChecker>();
        }

        private void OnEnable()
        {
            _groundChecker.GroundChanged += OnGroundChanged;
            _groundChecker.BackWheelGroundChanged += OnBackWheelGroundChanged;
        }

        private void OnDisable()
        {
            if (BehaviourCoroutine != null)
                StopCoroutine(BehaviourCoroutine);

            _groundChecker.GroundChanged -= OnGroundChanged;
            _groundChecker.BackWheelGroundChanged -= OnBackWheelGroundChanged;
        }

        protected abstract void Inject(BikeBehaviourInject inject);

        protected void Init(BikeBehaviourInject inject)
        {
            Player = inject.Player;
            BikeBody = inject.BikeBody.transform;
        }

        private void OnGroundChanged(bool value) => IsGrounded = value;
        private void OnBackWheelGroundChanged(bool value) => IsBackWheelGrounded = value;
    }
}