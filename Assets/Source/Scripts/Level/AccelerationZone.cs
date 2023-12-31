﻿using BikeDefied.UnityTool;
using UnityEngine;

namespace BikeDefied.LevelComponents
{
    public class AccelerationZone : MonoBehaviour
    {
        [SerializeField] private bool _accelerationMultiplyMode;

        [SerializeField] [VisibleToCondition(nameof(_accelerationMultiplyMode), true)] private float _accelerationMultiply;

        [SerializeField] [VisibleToCondition(nameof(_accelerationMultiplyMode), false)] private float _speed;

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out IAccelerationable component))
            {
                if (_accelerationMultiplyMode)
                {
                    component.UpdateAccelerationMultiply = _accelerationMultiply;
                }
                else
                {
                    component.SelfRigidbody.AddForce(transform.forward * _speed * Time.deltaTime, ForceMode.VelocityChange);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IAccelerationable component))
            {
                if (_accelerationMultiplyMode)
                {
                    component.UpdateAccelerationMultiply = default;
                }
            }
        }
    }
}