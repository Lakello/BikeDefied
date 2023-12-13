using System;
using System.Collections.Generic;

namespace BikeDefied.FSM
{
    public abstract class StateMachine<TMachine> : IDisposable
        where TMachine : StateMachine<TMachine>
    {
        private Dictionary<Type, State<TMachine>> _states;

        protected StateMachine(Func<Dictionary<Type, State<TMachine>>> getStates) =>
            _states = getStates();

        public State<TMachine> CurrentState { get; private set; }

        public void Dispose() =>
            CurrentState?.Exit();

        public virtual void EnterIn<TState>()
            where TState : State<TMachine>
        {
            if (_states.ContainsKey(typeof(TState)) == false)
            {
                throw new NullReferenceException(nameof(_states));
            }

            if (!_states.TryGetValue(typeof(TState), out State<TMachine> state))
            {
                return;
            }
            
            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();
        }

        protected State<TMachine> TryGetState(Type stateType) =>
            _states.TryGetValue(stateType, out State<TMachine> state) ? state : null;
    }
}