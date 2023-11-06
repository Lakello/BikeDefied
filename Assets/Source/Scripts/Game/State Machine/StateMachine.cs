using System.Collections.Generic;
using System;

namespace BikeDefied.FSM
{
    public abstract class StateMachine<TMachine> : IDisposable where TMachine : StateMachine<TMachine>
    {
        private Dictionary<Type, State<TMachine>> _states;

        public State<TMachine> CurrentState { get; private set; }

        public StateMachine(Func<Dictionary<Type, State<TMachine>>> getStates) => 
            _states = getStates();

        public void Dispose()
        {
            CurrentState?.Exit();
        }

        public virtual void EnterIn<TState>() where TState : State<TMachine>
        {
            if (_states.ContainsKey(typeof(TState)) == false)
                throw new NullReferenceException(nameof(_states));

            if (_states.TryGetValue(typeof(TState), out State<TMachine> state))
            {
                CurrentState?.Exit();
                CurrentState = state;
                CurrentState.Enter();
            }
        }

        protected State<TMachine> TryGetState(Type stateType)
        {
            if (_states.TryGetValue(stateType, out State<TMachine> state))
                return state;
            else
                return null;
        }
    }
}