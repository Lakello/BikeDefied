using IJunior.StateMachine;
using System.Collections;
using UnityEngine;

public class PlayState : GameState
{
    private readonly MonoBehaviour _context;
    private readonly PlayerInput _playerInput;
    private Coroutine _waitCoroutine;

    public PlayState(MonoBehaviour context, PlayerInput input)
    {
        _context = context;
        _playerInput = input;
    }

    public override void Enter()
    {
        GameStateMachine.Instance.SetWindow<PlayWindowState>();
        if (_playerInput == null)
            _waitCoroutine = _context.StartCoroutine(WaitInject());
        else 
            _playerInput.Enable();
    }

    public override void Exit()
    {
        if (_waitCoroutine != null)
            _context.StopCoroutine(_waitCoroutine);

        _playerInput.Disable();
    }

    private IEnumerator WaitInject()
    {
        yield return _playerInput != null;
        _playerInput.Enable();
    }
}