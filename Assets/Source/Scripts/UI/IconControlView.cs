using BikeDefied.BikeSystem;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace BikeDefied.UI
{
    public class IconControlView : MonoBehaviour
    {
        [SerializeField] private Image _leftArrow;
        [SerializeField] private Image _rightArrow;
        [SerializeField] private Image _frontFlip;
        [SerializeField] private Image _backFlip;

        private GroundChecker _checker;

        private void OnDisable()
        {
            if (_checker != null)
                _checker.GroundChanged -= OnGroundChanged;
        }

        private void OnGroundChanged(bool isGrouded)
        {
            if (isGrouded)
            {
                _leftArrow.gameObject.SetActive(true);
                _rightArrow.gameObject.SetActive(true);
                _frontFlip.gameObject.SetActive(false);
                _backFlip.gameObject.SetActive(false);
            }
            else
            {
                _leftArrow.gameObject.SetActive(false);
                _rightArrow.gameObject.SetActive(false);
                _frontFlip.gameObject.SetActive(true);
                _backFlip.gameObject.SetActive(true);
            }
        }


        [Inject]
        private void Inject(GroundChecker checker)
        {
            _checker = checker;
            _checker.GroundChanged += OnGroundChanged;
        }
    }
}