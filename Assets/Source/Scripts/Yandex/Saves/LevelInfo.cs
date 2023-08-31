public struct LevelInfo : ILevelInfo
{
    private int _levelIndex;
    private int _bestScore;

    public int LevelIndex => _levelIndex;
    public int BestScore => _bestScore;

    public LevelInfo(int index, int score)
    {
        _levelIndex = index;
        _bestScore = score;
    }
}