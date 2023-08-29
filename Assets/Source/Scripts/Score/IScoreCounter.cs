public interface IScoreCounter
{
    public abstract event System.Action<IReward> ScoreAdd;
}