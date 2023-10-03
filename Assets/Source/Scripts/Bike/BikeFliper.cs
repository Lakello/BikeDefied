using Reflex.Attributes;
using UnityEngine;

public class BikeFliper : BikeBehaviour
{
    [SerializeField] private float _rotateSpeed;

    [SerializeField] private Rigidbody _backWheel;
    [SerializeField] private Rigidbody _frontWheel;

    private Rigidbody _bikeRigidbody;

    private void Start()
    {
        _bikeRigidbody = BikeBody.GetComponent<Rigidbody>();

        //BehaviourCoroutine = StartCoroutine(Player.Behaviour(
        //condition: () =>
        //{
        //    return !IsGrounded;
        //},
        //action: () =>
        //{
        //    var horizontal = InputHandler.Horizontal;

        //    if (horizontal != 0)
        //        Flip(horizontal);
        //    else if (_bikeRigidbody.isKinematic == false)
        //        _bikeRigidbody.angularVelocity = new Vector3(0, _bikeRigidbody.velocity.y, _bikeRigidbody.velocity.z);
        //}));
    }

    private void FixedUpdate()
    {
        if (!IsGrounded)
        {
            var horizontal = InputHandler.Horizontal;

            if (horizontal != 0)
                Flip(horizontal);
            else if (_bikeRigidbody.isKinematic == false)
                _bikeRigidbody.angularVelocity = new Vector3(0, _bikeRigidbody.velocity.y, _bikeRigidbody.velocity.z);
        }    
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

    private void Flip(float direction)
    {
        _backWheel.AddForce(_rotateSpeed * -direction * Time.fixedDeltaTime * BikeBody.up, ForceMode.VelocityChange);
        _frontWheel.AddForce(_rotateSpeed * direction * Time.fixedDeltaTime * BikeBody.up, ForceMode.VelocityChange);
    }
}