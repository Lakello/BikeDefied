using UnityEngine;

namespace IJunior.StateMachine
{
    public abstract class State<TMachine> : MonoBehaviour where TMachine : StateMachine<TMachine>
    {
        protected TMachine StateMachine;

        private void Awake()
        {
            StateMachine = (TMachine)GetComponent<StateMachine<TMachine>>();

            StateMachine.AddState(this);
        }

        public abstract void Enter();
        public abstract void Exit();
    }
}