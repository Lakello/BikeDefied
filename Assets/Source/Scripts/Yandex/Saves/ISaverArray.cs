public interface ISaverArray<TSave>
{
    public event System.Action<TSave> ValueUpdated;

    public TSave Get(int index);
    public void Set(TSave value);
}