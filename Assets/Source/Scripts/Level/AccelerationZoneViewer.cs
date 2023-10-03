using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AccelerationZoneViewer : MonoBehaviour
{
    [SerializeField] private BoxCollider _selfCollider;
    [SerializeField] private SpriteRenderer _elementPrefab;
    [SerializeField] private float _speedAlphaChanged;

    [SerializeField, Range(0f, 1f)] private float _minAlpha;
    [SerializeField, Range(0f, 1f)] private float _maxAlpha;

    [SerializeField] private float _timeChangeCurrentIndex;
    [SerializeField] private bool _isUseParentScaleY;

    private List<SpriteRenderer> _elements = new();
    private int _countElements;
    private Color _color;
    private int _currentIndex;
    private float _time;

    private void Start()
    {
        CreateElements();

        SetTransform();
    }

    private void Update()
    {
        _time += Time.deltaTime;

        if (_time >= _timeChangeCurrentIndex)
        {
            _currentIndex = (int)Mathf.Repeat(++_currentIndex, _elements.Count);
            _time = 0;
        }

        UpdateAlpha();
    }

    private void CreateElements()
    {
        float alpha = _maxAlpha;
        _countElements = (int)_selfCollider.size.z;

        for (int i = 0; i < _countElements; i++)
        {
            var element = Instantiate(_elementPrefab, transform);
            _elements.Add(element);

            _color = element.color;

            alpha -= (_maxAlpha - _minAlpha) / _countElements;
            _color.a = alpha;
            element.color = _color;
        }
    }

    private void SetTransform()
    {
        Vector3 currentPosition = transform.position + (-transform.forward * (_selfCollider.size.z / 2));
        Vector3 scale = new Vector3()
        {
            x = 1,
            y = _isUseParentScaleY ? _selfCollider.size.y : 1,
            z = 1,
        };

        foreach (var element in _elements)
        {
            element.transform.position = currentPosition;
            element.transform.localScale = scale;
            currentPosition += transform.forward;
        }
    }

    private void UpdateAlpha()
    {
        for (int i = 0; i < _elements.Count; i++)
        {
            _color = _elements[i].color;

            if (i == _currentIndex)
                _color.a = _maxAlpha;
            else
                _color.a -= _speedAlphaChanged * Time.deltaTime;

            _elements[i].color = _color;
        }
    }
}