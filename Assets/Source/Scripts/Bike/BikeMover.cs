using Reflex.Attributes;
using Source;
using UnityEngine;

public class BikeMover : BikeBehaviour
{
    [SerializeField] private float _force = 50;

    private Rigidbody _bikeRigidbody;

    private void Start()
    {
        _bikeRigidbody = BikeBody.GetComponent<Rigidbody>();

        BehaviourCoroutine = StartCoroutine(Player.Behaviour(
        condition: () =>
        {
            return IsGrounded;
        },
        action: () =>
        {
            var horizontal = InputHandler.Horizontal;

            if (horizontal != 0)
                Move(horizontal);
        }));
    }

    [Inject]
    protected override void Inject(BikeBehaviourInject inject)
    {
        Init(inject);
    }

    private void Move(float value)
    {
        _bikeRigidbody.AddForce(new Vector3(0, 0, _force * value * Time.deltaTime), ForceMode.Acceleration);
    }
}