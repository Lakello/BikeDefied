using System;

public class Transition<TMachine, TTargetState> where TMachine : StateMachine<TMachine> where TTargetState : State<TMachine>
{
    private StateMachine<TMachine> _machine;

    public Transition(StateMachine<TMachine> stateMachine)
    {
        _machine = stateMachine;
    }

    public void Transit()
    {
        _machine.EnterIn<TTargetState>();
    }
}