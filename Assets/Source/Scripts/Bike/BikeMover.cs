using Reflex.Attributes;
using System.Collections;
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
        StartCoroutine(Behaviour(
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
    protected override void Inject(IInputHandler input)
    {
        InputHandler = input;
    }

    private void Move(float value)
    {
        _motor.targetVelocity = _targetVelocity * value;
        _motor.force = _force;

        _backWheel.useMotor = true;
        _backWheel.motor = _motor;
    }

    private void Stop()
    {
        _motor.force = _brakeForce;
        _motor.targetVelocity = 0;

        _backWheel.useMotor = true;
        _backWheel.motor = _motor;
    }
}
