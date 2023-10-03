using UnityEngine;
#if UNITY_EDITOR
using UnityTool;
#endif

public class AccelerationZone : MonoBehaviour
{
    [SerializeField] private bool _accelerationMultiplyMode;
#if UNITY_EDITOR
    [VisibleIfTrue(nameof(_accelerationMultiplyMode))]
#endif
    [SerializeField] private float _accelerationKoef;

#if UNITY_EDITOR
    [VisibleIfFalse(nameof(_accelerationMultiplyMode))]
#endif
    [SerializeField] private float _speed;


    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out IAccelerationable component))
        {
            if (_accelerationMultiplyMode)
                component.UpdateAccelerationKoef = _accelerationKoef;
            else
                component.SelfRigidbody.AddForce(transform.forward * _speed * Time.deltaTime, ForceMode.VelocityChange);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IAccelerationable component))
        {
            if (_accelerationMultiplyMode)
                component.UpdateAccelerationKoef = default;
        }
    }
}