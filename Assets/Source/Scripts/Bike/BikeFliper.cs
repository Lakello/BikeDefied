using Reflex.Attributes;
using System.Collections;
using UnityEngine;

public class BikeFliper : BikeBehaviour
{
    [SerializeField] private float _rotateSpeed;

    [SerializeField] private Transform _bike;
    [SerializeField] private Rigidbody _bikeRigidbody;

    private void Start()
    {
        StartCoroutine(Behaviour(
        condition: () =>
        {
            if (IsGrounded)
            {
                ResetFlipRigidbody();
                return true;
            }

            return false;
        },
        action: () =>
        {
            SetFlipRigidbody();

            var horizontal = InputHandler.Horizontal;

            if (horizontal != 0)
                Flip(horizontal);
        }));
    }

    [Inject]
    protected override void Inject(IInputHandler input)
    {
        InputHandler = input;
    }

    private void Flip(float direction)
    {
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(_rotateSpeed * -direction * Time.deltaTime, 0, 0));

        _bike.rotation *= deltaRotation;
    }

    private void SetFlipRigidbody()
    {
        _bikeRigidbody.constraints = RigidbodyConstraints.FreezeRotation |
                                     RigidbodyConstraints.FreezePositionX;
    }

    private void ResetFlipRigidbody()
    {
        _bikeRigidbody.constraints = RigidbodyConstraints.FreezeRotationY |
                                     RigidbodyConstraints.FreezeRotationZ |
                                     RigidbodyConstraints.FreezePositionX;
    }
}
