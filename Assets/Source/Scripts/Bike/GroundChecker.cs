using System;
using UnityEngine;

namespace BikeDefied.BikeSystem
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private Collider _backCollider;
        [SerializeField] private Collider _frontCollider;

        private float _checkRadius = 0.31f;
        private bool _isGrounded;
        private bool _isBackWheelGround;
        private bool _isFrontWheelGround;

        public event Action<bool> GroundChanged;
        public event Action<bool> BackWheelGroundChanged;

        private void FixedUpdate()
        {
            TryChangeGround(ref _isBackWheelGround, IsGround(_backCollider), BackWheelGroundChanged);
            TryChangeGround(ref _isFrontWheelGround, IsGround(_frontCollider));
            TryChangeGround(ref _isGrounded, _isBackWheelGround || _isFrontWheelGround, GroundChanged);
        }

        private void TryChangeGround(ref bool isGround, bool condition, Action<bool> action = null)
        {
            if (isGround != condition)
            {
                isGround = condition;
                action?.Invoke(isGround);
            }
        }

        private bool IsGround(Collider collider) =>
            Physics.CheckCapsule(
                collider.bounds.center,
                collider.bounds.center,
                _checkRadius,
                _groundMask,
                QueryTriggerInteraction.Ignore);
    }
}