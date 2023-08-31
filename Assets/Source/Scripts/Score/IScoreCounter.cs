public interface IScoreCounter
{
    public abstract string Name { get; }

    public abstract event System.Action<IReward> ScoreAdd;
}