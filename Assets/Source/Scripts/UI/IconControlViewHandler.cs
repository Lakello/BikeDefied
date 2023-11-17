using BikeDefied.BikeSystem;
using Reflex.Attributes;
using UnityEngine;

namespace BikeDefied.UI
{
    public class IconControlViewHandler : MonoBehaviour
    {
        [SerializeField] private ControlView _moveControlView;
        [SerializeField] private ControlView _flipControlView;

        private GroundChecker _checker;

        [Inject]
        private void Inject(GroundChecker checker)
        {
            _checker = checker;
            _checker.GroundChanged += OnGroundChanged;
        }

        private void OnDisable()
        {
            if (_checker != null)
            {
                _checker.GroundChanged -= OnGroundChanged;
            }
        }

        private void OnGroundChanged(bool isGrouded)
        {
            if (isGrouded)
            {
                _moveControlView.Enable();
                _flipControlView.Disable();
            }
            else
            {
                _moveControlView.Disable();
                _flipControlView.Enable();
            }
        }
    }
}