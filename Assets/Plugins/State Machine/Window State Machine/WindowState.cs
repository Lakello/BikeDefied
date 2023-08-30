using UnityEngine;

namespace IJunior.StateMachine
{
    public abstract class WindowState : State<WindowStateMachine>
    {
        [SerializeField] private Window _window;

        public override void Enter()
        {
            UnityEngine.Debug.Log($"Window State = {StateMachine != null}");
            _window.gameObject.SetActive(true);
        }

        public override void Exit()
        {
            _window.gameObject.SetActive(false);
        }
    }
}