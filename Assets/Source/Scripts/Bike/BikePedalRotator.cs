using Reflex.Attributes;
using UnityEngine;

namespace BikeDefied.BikeSystem
{
    public class BikePedalRotator : MonoBehaviour
    {
        [SerializeField] private Transform _leftPedal;
        [SerializeField] private Transform _rightPedal;
        [SerializeField] private AnimationCurve _rotationCurve;
        [SerializeField] private float _maxVelocity;

        private Rigidbody _bikeRigidbody;
        private float _angle;

        private float NormalVelocity => Mathf.Abs(_bikeRigidbody.velocity.z / _maxVelocity);
        
        private float Direction => Mathf.Clamp(_bikeRigidbody.velocity.z, -1, 1);

        [Inject]
        private void Inject(Bike bike) =>
            _bikeRigidbody = bike.GetComponent<Rigidbody>();

        private void Update()
        {
            _angle = _rotationCurve.Evaluate(NormalVelocity);

            transform.rotation *= Quaternion.AngleAxis(_angle * -Direction, Vector3.right);

            _leftPedal.rotation = _rightPedal.rotation
                = Quaternion.LookRotation(_bikeRigidbody.transform.forward, Vector3.up);
        }
    }
}