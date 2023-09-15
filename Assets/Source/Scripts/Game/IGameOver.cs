public interface IGameOver
{
    public event System.Func<bool> GameOver;
    public event System.Action LateGameOver;
}
