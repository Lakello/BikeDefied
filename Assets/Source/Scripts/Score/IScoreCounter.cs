public interface IScoreCounter
{
    public abstract event System.Action<float> ScoreUpdated;
}