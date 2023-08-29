using UnityEngine;

public abstract class WindowState : State<WindowStateMachine>
{
    [SerializeField] private Window _window;

    public override void Enter()
    {
        _window.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        _window.gameObject.SetActive(false);
    }
}