using UnityEngine;

internal interface IAccelerationable
{
    public Rigidbody SelfRigidbody { get; }
    public float UpdateAccelerationKoef { set; }
}