public interface ISaver<TSave>
{
    public event System.Action<TSave> ValueUpdated;

    public TSave Get();
    public void Set(TSave value);
}