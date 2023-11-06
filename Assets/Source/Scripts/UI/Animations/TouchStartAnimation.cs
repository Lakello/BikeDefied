using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BikeDefied.UI.Animations
{
    public class TouchStartAnimation : MonoBehaviour
    {
        [SerializeField] private Sprite _touch;
        [SerializeField] private Sprite _notTouch;
        [SerializeField] private Image _image;

        [SerializeField] private float _durationTouch;
        [SerializeField] private float _durationNotTouch;

        private Coroutine _animationCoroutine;

        private void OnEnable() =>
            _animationCoroutine = StartCoroutine(Animation());

        private void OnDisable() =>
            StopCoroutine(_animationCoroutine);

        private IEnumerator Animation()
        {
            WaitForSeconds touchTime = new(_durationTouch);
            WaitForSeconds notTouchTime = new(_durationNotTouch);

            while (enabled)
            {
                _image.sprite = _notTouch;

                yield return notTouchTime;

                _image.sprite = _touch;

                yield return touchTime;
            }
        }
    }
}