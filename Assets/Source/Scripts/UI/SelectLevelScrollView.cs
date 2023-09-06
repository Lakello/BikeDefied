using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;
using Reflex.Attributes;

[RequireComponent(typeof(ScrollRect))]
public class SelectLevelScrollView : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler
{
    [SerializeField] private float _toCenterTime = 0.5f;
    [SerializeField] private float _scaleTime = 0.5f;
    [SerializeField] private float _centerScale = 1f;
    [SerializeField] private float _unCenterScale = 0.9f;

    private HorizontalLayoutGroup _layoutGroup;
    private ScrollRect _scrollView;
    private Transform _content;
    private List<float> _childrenPositions = new List<float>();
    private float _targetPosition;

    private int _currentCenterChildIndex;

    private System.Func<int> GetCurrentLevel;

    public event System.Action<int> LevelChanged;

    private void OnEnable()
    {
        Check.IfNotNullThen(_layoutGroup, () => _layoutGroup.LayoutUpdated += UpdateContentPosition);
    }

    private void OnDisable()
    {
        _layoutGroup.LayoutUpdated -= UpdateContentPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateTargetPositon();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _scrollView.StopMovement();

        UpdateTargetPositon();
        _content.DOLocalMoveX(_targetPosition, _toCenterTime);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _content.DOKill();
    }

    [Inject]
    private void Inject(IRead<CurrentLevel> read)
    {
        GetCurrentLevel = () => read.Read().Index;

        Init();

        UpdateContentPosition();
    }

    private void Init()
    {
        _scrollView = GetComponent<ScrollRect>();

        _content = _scrollView.content;

        if (!_content.TryGetComponent(out _layoutGroup))
        {
            _layoutGroup = _content.AddComponent<HorizontalLayoutGroup>();
        }

        _layoutGroup.LayoutUpdated += UpdateContentPosition;

        _scrollView.movementType = ScrollRect.MovementType.Unrestricted;
    }

    private void UpdateContentPosition()
    {
        CalculateContentPosition();

        SetContentPosition();
    }

    private void CalculateContentPosition()
    {
        float spacing;
        float widthMultiplier = 0.5f;

        float childPositionX = _scrollView.GetComponent<RectTransform>().rect.width * widthMultiplier - GetChildItemWidth(0) * widthMultiplier;
        spacing = _layoutGroup.Spacing;

        _childrenPositions.Clear();
        _childrenPositions.Add(childPositionX);

        for (int i = 1; i < _content.childCount; i++)
        {
            childPositionX -= GetChildItemWidth(i) * widthMultiplier + GetChildItemWidth(i - 1) * widthMultiplier + spacing;
            _childrenPositions.Add(childPositionX);
        }
    }

    private void SetContentPosition()
    {
        _content.DOKill();

        _currentCenterChildIndex = GetCurrentLevel();
        _targetPosition = _childrenPositions[_currentCenterChildIndex];

        _content.DOLocalMoveX(_targetPosition, _toCenterTime);
        SetCellScale();
    }

    private void UpdateTargetPositon()
    {
        _targetPosition = FindClosestChildPosition(_content.localPosition.x);
        LevelChanged?.Invoke(_currentCenterChildIndex);
        SetCellScale();
    }

    private void SetCellScale()
    {
        GameObject centerChild;
        for (int i = 0; i < _content.childCount; i++)
        {
            centerChild = _content.GetChild(i).gameObject;
            centerChild.transform.DOKill();
            if (i == _currentCenterChildIndex)
                centerChild.transform.DOScale(_centerScale * Vector3.one, _scaleTime);
            else
                centerChild.transform.DOScale(_unCenterScale * Vector3.one, _scaleTime);
        }
    }

    private float FindClosestChildPosition(float currentPosition)
    {
        float closest = 0;
        float maxDistance = Mathf.Infinity;
        for (int i = 0; i < _childrenPositions.Count; i++)
        {
            float position = _childrenPositions[i];
            float distance = Mathf.Abs(position - currentPosition);
            if (distance < maxDistance)
            {
                maxDistance = distance;
                closest = position;
                _currentCenterChildIndex = i;
            }
            else
                break;
        }
        return closest;
    }

    private float GetChildItemWidth(int index)
    {
        return (_content.GetChild(index) as RectTransform).sizeDelta.x;
    }
}