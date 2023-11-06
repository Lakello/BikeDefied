using UnityEngine;

namespace BikeDefied.LevelComponents
{
    internal interface IAccelerationable
    {
        public Rigidbody SelfRigidbody { get; }
        public float UpdateAccelerationMultiply { set; }
    }
}