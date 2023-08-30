using System.Collections.Generic;
using System;
using UnityEngine;
using IJunior.TypedScenes;
using Unity.VisualScripting;

namespace IJunior.StateMachine
{
    public abstract class StateMachine<TMachine> : MonoBehaviour where TMachine : StateMachine<TMachine>
    {
        [SerializeField] private List<State<TMachine>> _states;

        protected Dictionary<Type, State<TMachine>> States = new Dictionary<Type, State<TMachine>>();
        protected State<TMachine> CurrentState;

        protected abstract TMachine SelfType { get; }

        private void OnDisable()
        {
            CurrentState?.Exit();
        }

        protected void Init(ref TMachine instance)
        {
            if (instance == null)
            {
                instance = SelfType;

                if (_states != null && _states.Count > 0)
                {
                    foreach (var state in _states)
                    {
                        state.Init(instance);

                        AddState(state);
                    }
                }
            }
            else
                Destroy(gameObject);
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

        protected void AddState(State<TMachine> state)
        {
            Type type = state.GetType();

            if (States.ContainsKey(type) == false)
            {
                States.Add(type, state);
            }
        }
    }
}