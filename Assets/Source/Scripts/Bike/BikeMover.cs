using Reflex.Attributes;
using UnityEngine;

public class BikeMover : BikeBehaviour, IAccelerationable
{
    [SerializeField] private float _force = 50;
    [SerializeField, Range(1f, 10f)] private float _maxAccelerationKoef = 5f;

    private Rigidbody _bikeRigidbody;
    private float _accelerationKoef = 1f;

    public float UpdateAccelerationKoef { set => _accelerationKoef = Mathf.Clamp(value, 1f, _maxAccelerationKoef); }
    public Rigidbody SelfRigidbody => _bikeRigidbody;

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

    [Inject]
    private void Inject(IInputHandler inputHandler)
    {
        InputHandler = inputHandler;
    }

    private void Move(float value)
    {
        _bikeRigidbody.AddForce(new Vector3(0, 0, _force * value * _accelerationKoef * Time.deltaTime), ForceMode.VelocityChange);
    }
}