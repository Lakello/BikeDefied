using Reflex.Attributes;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BikeFliper : BikeBehaviour
{
    [SerializeField] private float _rotateSpeed;

    private Rigidbody _selfRigidbody;

    private void Start()
    {
        _selfRigidbody = GetComponent<Rigidbody>();
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

            SetFlipConstraints();

            var horizontal = InputHandler.Horizontal;

            if (horizontal != 0)
                Flip(horizontal);

            yield return null;
        }
    }

    private void Flip(float direction)
    {
        transform.Rotate(new Vector3(_rotateSpeed * -direction * Time.deltaTime, 0, 0));
    }

    private void SetFlipConstraints()
    {
        _selfRigidbody.constraints = RigidbodyConstraints.None;

        _selfRigidbody.constraints = RigidbodyConstraints.FreezeRotationX;
        _selfRigidbody.constraints = RigidbodyConstraints.FreezeRotationY;
        _selfRigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;

        _selfRigidbody.constraints = RigidbodyConstraints.FreezePositionX;
    }
}
