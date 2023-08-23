using UnityEngine;

public class BikeRigidbodyConstraints : IRead<RigidbodyConstraints>, IWrite<RigidbodyConstraints>
{
    private Rigidbody _bikeRigidbody;

    public BikeRigidbodyConstraints(Rigidbody bikeRigidbody)
    {
        _bikeRigidbody = bikeRigidbody;
    }

    public RigidbodyConstraints Read()
    {
        return _bikeRigidbody.constraints;
    }

    public void Write(RigidbodyConstraints value)
    {
        _bikeRigidbody.constraints = value;
    }
}