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
        
    }

    private void Flip(float direction)
    {
        
    }
}
