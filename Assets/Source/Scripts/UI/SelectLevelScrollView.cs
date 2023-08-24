﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

[RequireComponent(typeof(ScrollRect))]
public class SelectLevelScrollView : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler
{
    [SerializeField] private float _toCenterTime = 0.5f;
    [SerializeField] private float _scaleTime = 0.5f;
    [SerializeField] private float _centerScale = 1f;
    [SerializeField] private float _unCenterScale = 0.9f;

    private ScrollRect _scrollView;
    private Transform _content;
    private List<float> _childrenPositions = new List<float>();
    private float _targetPosition;

    private int _currentCenterChildIndex;

    public GameObject CurrentCenterChildItem
    {
        get
        {
            GameObject centerChild = null;
            if (_content != null && _currentCenterChildIndex >= 0 && _currentCenterChildIndex < _content.childCount)
            {
                centerChild = _content.GetChild(_currentCenterChildIndex).gameObject;
            }
            return centerChild;
        }
    }

    private void Awake()
    {
        _scrollView = GetComponent<ScrollRect>();

        _content = _scrollView.content;

        if (!_content.TryGetComponent(out HorizontalLayoutGroup layoutGroup))
        {
            layoutGroup = _content.AddComponent<HorizontalLayoutGroup>();
        }

        _scrollView.movementType = ScrollRect.MovementType.Unrestricted;

        float spacing;
        float widthMultiplier = 0.5f;

        float childPositionX = _scrollView.GetComponent<RectTransform>().rect.width * widthMultiplier - GetChildItemWidth(0) * widthMultiplier;
        spacing = layoutGroup.spacing;

        _childrenPositions.Add(childPositionX);

        for (int i = 1; i < _content.childCount; i++)
        {
            childPositionX -= GetChildItemWidth(i) * widthMultiplier + GetChildItemWidth(i - 1) * widthMultiplier + spacing;
            _childrenPositions.Add(childPositionX);
        }

        SetStartContentPosition();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _targetPosition = FindClosestChildPos(_content.localPosition.x, out _currentCenterChildIndex);
        SetCellScale();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _scrollView.StopMovement();
        _targetPosition = FindClosestChildPos(_content.localPosition.x, out _currentCenterChildIndex);
        _content.DOLocalMoveX(_targetPosition, _toCenterTime);
        SetCellScale();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _content.DOKill();
        _currentCenterChildIndex = -1;
    }

    private void SetStartContentPosition()
    {
        _content.DOKill();

        _targetPosition = _childrenPositions[_currentCenterChildIndex];

        _content.DOLocalMoveX(_targetPosition, _toCenterTime);
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

    private float FindClosestChildPos(float currentPosition, out int currentCenterChildIndex)
    {
        float closest = 0;
        float maxDistance = Mathf.Infinity;
        currentCenterChildIndex = -1;
        for (int i = 0; i < _childrenPositions.Count; i++)
        {
            float position = _childrenPositions[i];
            float distance = Mathf.Abs(position - currentPosition);
            if (distance < maxDistance)
            {
                maxDistance = distance;
                closest = position;
                currentCenterChildIndex = i;
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