[System.Serializable]
public class LevelInfo : IPlayerData
{
    [UnityEngine.SerializeField] private int _levelIndex;
    [UnityEngine.SerializeField] private int _bestScore;

    public int LevelIndex => _levelIndex;
    public int BestScore => _bestScore;

    public LevelInfo(int levelIndex, int bestScore)
    {
        _levelIndex = levelIndex;
        _bestScore = bestScore;
    }
}