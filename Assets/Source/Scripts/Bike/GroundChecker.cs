using System;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Collider _selfCollider;

    private float _checkRadius = 0.28f;
    private bool _isGround;

    public event Action<bool> GroundChanged;

    private void FixedUpdate()
    {
        Check();
    }

    private void Check()
    {
        bool isGround = IsGround();

        if (_isGround != isGround)
        {
            GroundChanged?.Invoke(isGround);
            _isGround = isGround;
        }
    }

    private bool IsGround()
    {
        return Physics.CheckCapsule(_selfCollider.bounds.center,
                                    _selfCollider.bounds.center,
                                    _checkRadius,
                                    _groundMask,
                                    QueryTriggerInteraction.Ignore);
    }
}