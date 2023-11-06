namespace BikeDefied.FSM.Game
{
    public struct GameStateInject
    {
        public IMenuStateChangeble Menu { get; private set; }
        public IPlayLevelStateChangeble PlayLevel { get; private set; }
        public IEndLevelStateChangeble EndLevel { get; private set; }

        public GameStateInject(System.Func<(IMenuStateChangeble, IPlayLevelStateChangeble, IEndLevelStateChangeble)> inject) => 
            (Menu, PlayLevel, EndLevel) = inject();
    }
}