using UnityEngine;
using UnityTool;

public class AccelerationZone : MonoBehaviour
{
    [SerializeField] private bool _accelerationMultiplyMode;
    [SerializeField, VisibleIfTrue(nameof(_accelerationMultiplyMode))] private float _accelerationKoef;
    [SerializeField, VisibleIfFalse(nameof(_accelerationMultiplyMode))] private float _speed;


    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out IAccelerationable component))
        {
            Debug.Log("STAY");

            if (_accelerationMultiplyMode)
                component.UpdateAccelerationKoef = _accelerationKoef;
            else
                component.SelfRigidbody.AddForce(transform.forward * _speed * Time.deltaTime, ForceMode.VelocityChange);
        }
    }
}