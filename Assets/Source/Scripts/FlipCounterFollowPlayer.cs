using Reflex.Attributes;
using UnityEngine;

public class FlipCounterFollowPlayer : MonoBehaviour
{
    private Transform _bike;

    [Inject]
    private void Inject(Bike bike)
    {
        _bike = bike.GetComponent<Transform>();
    }

    private void Update()
    {
        transform.position = _bike.position;
    }
}