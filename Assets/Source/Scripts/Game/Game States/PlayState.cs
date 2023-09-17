using IJunior.StateMachine;
using System;
using UnityEngine;

public class PlayState : GameState, IGamePlay
{
    private readonly MonoBehaviour _context;
    private readonly PlayerInput _playerInput;
    private readonly IAudioController _audioController;

    public PlayState(MonoBehaviour context, PlayerInput input, IAudioController audioController)
    {
        _context = context;
        _playerInput = input;
        _audioController = audioController;
    }

    public event Action GamePlay;

    public override void Enter()
    {
        GameStateMachine.Instance.SetWindow<PlayWindowState>();
        _playerInput.Enable();

        GamePlay?.Invoke();
        _audioController.Play(Audio.LevelPlay);
    }

    public override void Exit()
    {
        _playerInput.Disable();
    }
}