using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public abstract class State : MonoBehaviour
{
    protected StateMachine StateMachine;

    private void Start()
    {
        StateMachine = GetComponent<StateMachine>();

        StateMachine.AddState(this);
    }

    public abstract void Enter();
    public abstract void Exit();
}