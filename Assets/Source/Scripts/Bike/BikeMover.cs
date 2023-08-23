using Reflex.Attributes;
using UnityEngine;

public class BikeMover : BikeBehaviour
{
    [SerializeField] private HingeJoint _backWheel;
    [SerializeField] private float _targetVelocity = 25;
    [SerializeField] private float _force = 50;
    [SerializeField] private float _brakeForce = 50;

    private JointMotor _motor = new();

    private void Start()
    {
        BehaviourCoroutine = StartCoroutine(Player.Behaviour(
        condition: () =>
        {
            return !IsBackWheelGrounded;
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
    protected override void Inject(IInputHandler input, IGameOver game)
    {
        Init(input, game);
    }

    protected override void OnGameOver()
    {
        Stop();
        _backWheel.useMotor = false;
    }

    private void Move(float value)
    {
        UpdateMotor(_targetVelocity * value, _force);
    }

    private void Stop()
    {
        UpdateMotor(force: _brakeForce);
    }

    private void UpdateMotor(float targetVelocity = 0, float force = 0)
    {
        _motor.targetVelocity = targetVelocity;
        _motor.force = force;

        _backWheel.useMotor = true;
        _backWheel.motor = _motor;
    }
}
