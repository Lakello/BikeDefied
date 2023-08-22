using System;

public class Game : IGameOver
{
    public event Action GameOver;

    public void OnCrash()
    {
        GameOver?.Invoke();
    }
}
