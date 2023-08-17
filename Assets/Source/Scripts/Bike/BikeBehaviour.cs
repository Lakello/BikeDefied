using System;
using UnityEngine;

[RequireComponent(typeof(GroundChecker))]
public abstract class BikeBehaviour : MonoBehaviour
{
    protected IInputHandler InputHandler;
    protected bool IsGround { get; private set; }
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
    }

    private void OnDisable()
    {
        _groundChecker.GroundChanged -= OnGroundChanged;
    }

    protected abstract void Inject(IInputHandler input);

    private void OnGroundChanged(bool value) => IsGround = value;
}