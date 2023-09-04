using IJunior.StateMachine;
using System;

public class GameOverState : GameState, IGameOver
{
    public event Action GameOver;

    public override void Enter()
    {
        GameStateMachine.Instance.SetWindow<GameOverWindowState>();
        GameOver?.Invoke();
    }

    public override void Exit(){}
}