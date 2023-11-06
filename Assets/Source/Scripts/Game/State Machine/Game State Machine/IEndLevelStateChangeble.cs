namespace BikeDefied.FSM.Game
{
    public interface IEndLevelStateChangeble : IStateChangeble
    {
        public event System.Action LateStateChanged;
    }
}