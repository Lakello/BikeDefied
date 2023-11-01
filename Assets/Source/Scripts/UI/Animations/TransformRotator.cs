using UnityEngine;

namespace BikeDefied.UI.Animations
{
    public class TransformRotator : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private float _currentAngle;

        private void Update()
        {
            _currentAngle += _speed * Time.deltaTime;
            transform.localRotation = Quaternion.AngleAxis(_currentAngle, transform.forward);
        }
    }
}