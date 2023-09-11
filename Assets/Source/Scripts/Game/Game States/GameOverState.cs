using IJunior.StateMachine;
using System;

public class GameOverState : GameState, IGameOver
{
    public event Action GameOver;
    public event Action LateGameOver;

    public override void Enter()
    {
        GameStateMachine.Instance.SetWindow<GameOverWindowState>();
        GameOver?.Invoke();
        LateGameOver?.Invoke();
    }

    public override void Exit(){}
}