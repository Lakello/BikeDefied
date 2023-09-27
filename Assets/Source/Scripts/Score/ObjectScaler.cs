using System;
using System.Collections;
using UnityEngine;

public class ObjectScaler : MonoBehaviour
{
    [SerializeField] private Transform _object;
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float _animationTime;
    [SerializeField] private bool _isPlayToEnable;

    private Coroutine _animationCoroutine;

    private void OnEnable()
    {
        if (_isPlayToEnable)
            Play();
    }

    public void Play(Action<float> subAnimation = null, Action successAction = null)
    {
        if (_animationCoroutine != null)
            return;

        _animationCoroutine = StartCoroutine(Animation(subAnimation, successAction));
    }

    private IEnumerator Animation(Action<float> subAnimation = null, Action successAction = null)
    {
        var progress = 0f;

        while (progress <= _animationTime)
        {
            progress += Time.deltaTime;

            var animationValue = _animationCurve.Evaluate(progress / _animationTime);

            subAnimation?.Invoke(animationValue);

            _object.transform.localScale = new Vector3(animationValue, animationValue, animationValue);
            yield return null;
        }

        successAction?.Invoke();
        _animationCoroutine = null;
    }
}