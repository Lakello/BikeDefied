﻿using System;
using UnityEngine;

[RequireComponent(typeof(GroundChecker))]
public abstract class BikeBehaviour : MonoBehaviour
{
    protected IInputHandler InputHandler;
    protected bool IsGrounded { get; private set; }
    protected bool IsBackWheelGrounded { get; private set; }
    protected bool IsAlive { get; private set; }

    private GroundChecker _groundChecker;

    private void Awake()
    {
        _groundChecker = GetComponent<GroundChecker>();
        IsAlive = true;
    }

    private void OnEnable()
    {
        _groundChecker.GroundChanged += OnGroundChanged;
        _groundChecker.BackWheelGroundChanged += OnBackWheelGroundChanged;
    }

    private void OnDisable()
    {
        _groundChecker.GroundChanged -= OnGroundChanged;
        _groundChecker.BackWheelGroundChanged -= OnBackWheelGroundChanged;
    }

    protected abstract void Inject(IInputHandler input);

    private void OnGroundChanged(bool value) => IsGrounded = value;
    private void OnBackWheelGroundChanged(bool value) => IsBackWheelGrounded = value;
}