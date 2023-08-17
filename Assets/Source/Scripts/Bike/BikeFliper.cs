using Reflex.Attributes;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BikeFliper : BikeBehaviour
{
    [SerializeField] private float _rotateSpeed;

    [SerializeField] private Rigidbody _driveWheel;

    private void Start()
    {
        StartCoroutine(Fliper());
    }

    [Inject]
    protected override void Inject(IInputHandler input)
    {
        InputHandler = input;
    }

    private IEnumerator Fliper()
    {
        while (IsAlive)
        {
            if (IsGround)
            {
                yield return null;
                continue;
            }

            var horizontal = InputHandler.Horizontal;

            if (horizontal != 0)
                Flip(horizontal);

            yield return null;
        }
    }

    private void Flip(float direction)
    {
        _driveWheel.AddForce(transform.up * -direction * _rotateSpeed * Time.deltaTime, ForceMode.Force);
    }
}
