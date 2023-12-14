using System;
using System.Collections.Generic;
using BikeDefied.FSM;

namespace BikeDefied
{
    public class TransitionInitializer<TMachine>
        where TMachine : StateMachine<TMachine>
    {
        private readonly List<Subscription> _subscribtions = new List<Subscription>();
        private readonly TMachine _stateMachine;

        public TransitionInitializer(TMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void OnEnable()
        {
            if (_subscribtions != null)
            {
                Subscribe();
            }
        }

        public void OnDisable()
        {
            if (_subscribtions != null)
            {
                UnSubscribe();
            }
        }

        public void InitTransition<TTargetState>(ISubject subject, Action reloadScene = null)
            where TTargetState : State<TMachine>
        {
            var transition = new Transition<TMachine, TTargetState>(_stateMachine, reloadScene);

            subject.ActionEnded += transition.Transit;

            _subscribtions.Add(new Subscription(subject, transition.Transit));
        }

        private void Subscribe()
        {
            foreach (var action in _subscribtions)
            {
                action.Subject.ActionEnded += action.Observer;
            }
        }

        private void UnSubscribe()
        {
            foreach (var action in _subscribtions)
            {
                action.Subject.ActionEnded -= action.Observer;
            }
        }

        private struct Subscription
        {
            public ISubject Subject;
            public Action Observer;

            public Subscription(ISubject subject, Action observer)
            {
                Subject = subject;
                Observer = observer;
            }
        }
    }
}