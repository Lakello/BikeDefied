namespace BikeDefied.FSM
{
    public interface IStateChangeble
    {
        public event System.Func<bool> StateChanged;
    }
}
