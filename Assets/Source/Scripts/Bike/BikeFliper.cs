using Reflex.Attributes;
using UnityEngine;

public class BikeFliper : BikeBehaviour
{ 
    [SerializeField] private float _rotateSpeed;

    [SerializeField] private Transform _bike;

    private void Start()
    {
        BehaviourCoroutine = StartCoroutine(Player.Behaviour(
        condition: () =>
        {
            return !IsGrounded;
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
    protected override void Inject(BikeBehaviourInject inject)
    {
        Init(inject);
    }

    protected override void OnGameOver()
    {
        BikeRigidbodyConstraints.Write(RigidbodyConstraints.None);
    }

    private void Flip(float direction)
    {
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(_rotateSpeed * direction * Time.deltaTime, 0, 0));

        _bike.rotation *= deltaRotation;
    }

    private void SetFlipRigidbody()
    {
        BikeRigidbodyConstraints.Write(BikeRigidbodySetting.GetFlipConstraints());
    }
}
