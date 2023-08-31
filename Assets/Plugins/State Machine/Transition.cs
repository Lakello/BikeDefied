﻿using System;

namespace IJunior.StateMachine
{
    public class Transition<TMachine, TTargetState> where TMachine : StateMachine<TMachine> where TTargetState : State<TMachine>
    {
        private StateMachine<TMachine> _machine;
        private Action _reloadScene;

        public Transition(StateMachine<TMachine> stateMachine, Action reloadScene = null)
        {
            _machine = stateMachine;
            _reloadScene = reloadScene;
        }

        public void Transit()
        {
            _reloadScene?.Invoke();
            _machine.EnterIn<TTargetState>();
        }
    }
}