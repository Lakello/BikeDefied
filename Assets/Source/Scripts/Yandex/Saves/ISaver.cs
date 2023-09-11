public interface ISaver<TSave>
{
    public TSave Get();
    public void Set(TSave value);
}