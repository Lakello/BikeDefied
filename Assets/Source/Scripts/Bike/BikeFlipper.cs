using BikeDefied.InputSystem;
using Reflex.Attributes;
using UnityEngine;

namespace BikeDefied.BikeSystem
{
    public class BikeFlipper : BikeBehaviour
    {
        [SerializeField] private float _rotateSpeed;

        [SerializeField] private Rigidbody _backWheel;
        [SerializeField] private Rigidbody _frontWheel;

        private Rigidbody _bikeRigidbody;

        [Inject]
        protected override void Inject(BikeBehaviourInject inject) =>
            Init(inject);

        private void Start()
        {
            _bikeRigidbody = BikeBody.GetComponent<Rigidbody>();

            BehaviourCoroutine = StartCoroutine(Player.Behaviour(
            condition: () => !IsGrounded,
            action: () =>
            {
                float horizontal = InputHandler.Horizontal;

                if (horizontal != 0)
                {
                    Flip(horizontal);
                }
                else if (_bikeRigidbody.isKinematic == false)
                {
                    _bikeRigidbody.angularVelocity = new Vector3(0, _bikeRigidbody.velocity.y, _bikeRigidbody.velocity.z);
                }
            }));
        }

        private void Flip(float direction)
        {
            Vector3 force = _rotateSpeed * Time.deltaTime * BikeBody.up;

            _backWheel.AddForce(force * -direction, ForceMode.VelocityChange);
            _frontWheel.AddForce(force * direction, ForceMode.VelocityChange);
        }
    }
}