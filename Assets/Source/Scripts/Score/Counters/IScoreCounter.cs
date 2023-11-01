namespace BikeDefied.ScoreSystem
{
    public interface IScoreCounter
    {
        public abstract event System.Action<ScoreReward> ScoreAdd;
    }
}