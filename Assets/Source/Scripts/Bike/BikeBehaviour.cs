using BikeDefied.Game;
using BikeDefied.InputSystem;
using UnityEngine;

namespace BikeDefied.BikeSystem
{
    [RequireComponent(typeof(GroundChecker))]
    public abstract class BikeBehaviour : MonoBehaviour
    {
        private Coroutine _behaviourCoroutine;
        private GroundChecker _groundChecker;

        protected Coroutine BehaviourCoroutine
        {
            get => _behaviourCoroutine;
            set => _behaviourCoroutine ??= value;
        }
        
        protected Player Player { get; private set; }
        
        protected IInputHandler InputHandler { get; private set; }
        
        protected Transform BikeBody { get; private set; }
        
        protected bool IsGrounded { get; private set; }
        
        protected bool IsBackWheelGrounded { get; private set; }

        protected abstract void Inject(BikeBehaviourInject inject);

        protected void Init(BikeBehaviourInject inject)
        {
            Player = inject.Player;
            BikeBody = inject.BikeBody.transform;
            InputHandler = inject.InputHandler;
        }
        
        private void Awake()
        {
            _groundChecker = GetComponent<GroundChecker>();
            _groundChecker.GroundChanged += (value) => IsGrounded = value;
            _groundChecker.BackWheelGroundChanged += (value) => IsBackWheelGrounded = value;
        }

        private void OnDisable()
        {
            if (BehaviourCoroutine != null)
            {
                StopCoroutine(BehaviourCoroutine);
            }
        }
    }
}