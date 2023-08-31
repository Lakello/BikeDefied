using IJunior.StateMachine;
using Reflex.Attributes;
using System;
using System.Collections;
using UnityEngine;

public class PlayState : GameState
{
    private PlayerInput _playerInput;
    private Coroutine _waitCoroutine;

    public override void Enter()
    {
        StateMachine.SetWindow<PlayWindowState>();
        if (_playerInput == null)
            _waitCoroutine = StartCoroutine(WaitInject());
        else 
            _playerInput.Enable();
    }

    public override void Exit()
    {
        if (_waitCoroutine != null)
            StopCoroutine(_waitCoroutine);

        _playerInput.Disable();
    }

    [Inject]
    private void Inject(PlayerInput input)
    {
        _playerInput = input;
    }

    private IEnumerator WaitInject()
    {
        yield return _playerInput != null;
        _playerInput.Enable();
    }
}