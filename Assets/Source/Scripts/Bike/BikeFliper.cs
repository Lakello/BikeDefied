using Reflex.Attributes;
using UnityEngine;

public class BikeFliper : BikeBehaviour
{ 
    [SerializeField] private float _rotateSpeed;

    [SerializeField] private Transform _bike;
    [SerializeField] private Rigidbody _bikeRigidbody;

    private void Start()
    {
        BehaviourCoroutine = StartCoroutine(Player.Behaviour(
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
    protected override void Inject(IInputHandler input, IGameOver game)
    {
        Init(input, game);
    }

    protected override void OnGameOver()
    {
        _bikeRigidbody.constraints = RigidbodyConstraints.None;
    }

    private void Flip(float direction)
    {
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(_rotateSpeed * direction * Time.deltaTime, 0, 0));

        _bike.rotation *= deltaRotation;
    }

    private void SetFlipRigidbody()
    {
        _bikeRigidbody.constraints = BikeRigidbodySetting.GetFlipConstraints();
    }

    private void ResetFlipRigidbody()
    {
        _bikeRigidbody.constraints = BikeRigidbodySetting.GetMoveConstraints();
    }
}
