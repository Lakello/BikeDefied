using Reflex.Attributes;
using UnityEngine;

public class BikeMover : BikeBehaviour
{
    [SerializeField] private Rigidbody _bikeRigidbody;
    [SerializeField] private float _force = 50;

    private void Start()
    {
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
            else
                Stop();
        }));
    }

    [Inject]
    protected override void Inject(BikeBehaviourInject inject)
    {
        Init(inject);
    }

    protected override void OnGameOver()
    {

    }

    private void Move(float value)
    {
        _bikeRigidbody.AddForce(new Vector3(0, 0, _force * value * Time.deltaTime), ForceMode.Acceleration);
    }

    private void Stop()
    {

    }
}
