using BikeDefied.LevelComponents;
using UnityEngine;

namespace BikeDefied.BikeSystem
{
    public class BikeMover : BikeBehaviour, IAccelerationable
    {
        [SerializeField] private float _force = 50;

        private float _accelerationMultiply = 1f;

        public float UpdateAccelerationMultiply { set => _accelerationMultiply = Mathf.Clamp(value, 1f, 5f); }

        public Rigidbody SelfRigidbody { get; private set; }

        private void Start()
        {
            SelfRigidbody = BikeBody.GetComponent<Rigidbody>();

            BehaviourCoroutine = StartCoroutine(Player.Behaviour(
                condition: () => IsGrounded,
                action: () =>
                {
                    float horizontal = InputHandler.Horizontal;

                    if (horizontal != 0)
                    {
                        Move(horizontal);
                    }
                }));
        }

        private void Move(float value)
        {
            float force = _force * value * _accelerationMultiply * Time.deltaTime;

            SelfRigidbody.AddForce(new Vector3(0, 0, force), ForceMode.VelocityChange);
        }
    }
}