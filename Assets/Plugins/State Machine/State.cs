using UnityEngine;

namespace IJunior.StateMachine
{
    public abstract class State<TMachine> : MonoBehaviour where TMachine : StateMachine<TMachine>
    {
        protected TMachine StateMachine;

        public void Init(TMachine machine)
        {
            StateMachine = machine;
        }

        public abstract void Enter();
        public abstract void Exit();
    }
}