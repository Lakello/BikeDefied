public interface IScoreCounter
{
    public abstract event System.Action<int> ScoreChanged;
}