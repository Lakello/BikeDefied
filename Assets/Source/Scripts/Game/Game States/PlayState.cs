using IJunior.StateMachine;
using System;
using UnityEngine;

public class PlayState : GameState, IGamePlay
{
    private readonly MonoBehaviour _context;
    private readonly PlayerInput _playerInput;

    public PlayState(MonoBehaviour context, PlayerInput input)
    {
        _context = context;
        _playerInput = input;
    }

    public event Action GamePlay;

    public override void Enter()
    {
        GameStateMachine.Instance.SetWindow<PlayWindowState>();
        _playerInput.Enable();

        GamePlay?.Invoke();
    }

    public override void Exit()
    {
        _playerInput.Disable();
    }
}