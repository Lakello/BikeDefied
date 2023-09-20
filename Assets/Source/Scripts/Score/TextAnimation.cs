using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float _animationTime;

    private Coroutine _animationCoroutine;

    public void Play(string message, Action successAction = null)
    {
        _text.text = message;

        if (_animationCoroutine != null)
            return;

        _animationCoroutine = StartCoroutine(Animation());
    }

    private IEnumerator Animation(Action successAction = null)
    {
        var progress = 0f;

        while (progress <= _animationTime)
        {
            progress += Time.deltaTime;

            var animationValue = _animationCurve.Evaluate(progress / _animationTime);

            _text.alpha = animationValue;
            _text.transform.localScale = new Vector3(animationValue, animationValue, animationValue);
            yield return null;
        }

        successAction?.Invoke();
        _animationCoroutine = null;
    }
}