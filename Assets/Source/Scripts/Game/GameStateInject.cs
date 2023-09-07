public struct GameStateInject
{
    public IGameMenu Menu { get; private set; }
    public IGamePlay Play { get; private set; }
    public IGameOver Over { get; private set; }

    public GameStateInject(System.Func<(IGameMenu, IGamePlay, IGameOver)> inject) => (Menu, Play, Over) = inject();
}