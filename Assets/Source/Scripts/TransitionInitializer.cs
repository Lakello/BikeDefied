using System;
using System.Collections.Generic;
using IJunior.StateMachine;

public class TransitionInitializer<TMachine> where TMachine : StateMachine<TMachine>
{
    private TMachine _stateMachine;
    private List<Subscriber> _subscribes = new List<Subscriber>();

    private struct Subscriber
    {
        public ISubscribe Subscribe;
        public Action Action;

        public Subscriber(ISubscribe subscribe, Action action)
        {
            Subscribe = subscribe;
            Action = action;
        }
    }

    public TransitionInitializer(TMachine stateMachine) => _stateMachine = stateMachine;

    public void OnEnable()
    {
        if (_subscribes != null)
            Subscribe();
    }

    public void OnDisable()
    {
        if (_subscribes != null)
            UnSubscribe();
    }

    public void InitTransition<TTargetState>(ISubscribe subscribe) where TTargetState : State<TMachine>
    {
        var transition = new Transition<TMachine, TTargetState>(_stateMachine);

        subscribe.Action += transition.Transit;

        _subscribes.Add(new Subscriber(subscribe, transition.Transit));
    }

    private void Subscribe()
    {
        foreach (var subscriber in _subscribes)
            subscriber.Subscribe.Action += subscriber.Action;
    }

    private void UnSubscribe()
    {
        foreach (var subscriber in _subscribes)
            subscriber.Subscribe.Action -= subscriber.Action;
    }
}