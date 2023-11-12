using BikeDefied.InputSystem;
using BikeDefied.LevelComponents;
using Reflex.Attributes;
using UnityEngine;

namespace BikeDefied.BikeSystem
{
    public class BikeMover : BikeBehaviour, IAccelerationable
    {
        [SerializeField] private float _force = 50;

        private Rigidbody _bikeRigidbody;
        private float _accelerationMultiply = 1f;

        public float UpdateAccelerationMultiply { set => _accelerationMultiply = Mathf.Clamp(value, 1f, 5f); }
        public Rigidbody SelfRigidbody => _bikeRigidbody;

        [Inject]
        protected override void Inject(BikeBehaviourInject inject) =>
            Init(inject);

        [Inject]
        private void Inject(IInputHandler inputHandler) =>
            InputHandler = inputHandler;

        private void Start()
        {
            _bikeRigidbody = BikeBody.GetComponent<Rigidbody>();

            BehaviourCoroutine = StartCoroutine(Player.Behaviour(
            condition: () =>
            {
                return IsGrounded;
            },
            action: () =>
            {
                var horizontal = InputHandler.Horizontal;

                if (horizontal != 0)
                    Move(horizontal);
            }));
        }

        private void Move(float value)
        {
            var force = _force * value * _accelerationMultiply * Time.deltaTime;

            _bikeRigidbody.AddForce(new Vector3(0, 0, force), ForceMode.VelocityChange);
        }
    }
}