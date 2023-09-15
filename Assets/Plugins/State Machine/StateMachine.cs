using System.Collections.Generic;
using System;

namespace IJunior.StateMachine
{
    public abstract class StateMachine<TMachine> : IDisposable where TMachine : StateMachine<TMachine>
    {
        private List<State<TMachine>> _states;

        protected Dictionary<Type, State<TMachine>> States;

        public State<TMachine> CurrentState { get; private set; }

        public StateMachine(Func<Dictionary<Type, State<TMachine>>> getStates) => States = getStates();

        public void Dispose()
        {
            CurrentState?.Exit();
        }

        public void EnterIn<TState>() where TState : State<TMachine>
        {
            if (States.ContainsKey(typeof(TState)) == false)
                throw new NullReferenceException(nameof(States));

            if (States.TryGetValue(typeof(TState), out State<TMachine> state))
            {
                CurrentState?.Exit();
                CurrentState = state;
                CurrentState.Enter();
            }
        }
    }
}