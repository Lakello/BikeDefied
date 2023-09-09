using UnityEngine;

public sealed class Saves : MonoBehaviour, IReadFromArray<LevelInfo>, IRead<CurrentLevel>, IWrite<LevelInfo>, IWrite<CurrentLevel>
{
    public LevelInfo Read(int index) => default;//YandexGame.savesData.LevelInfo[index];

    public CurrentLevel Read() => default;// YandexGame.savesData.CurrentLevelIndex;

    public void Write(LevelInfo value)
    {
        if (ContainsLevelIndex(value.LevelIndex, out int index))
            UpdateLevelInfo(value, index);
        else
            AddLevelInfo(value);
    }
    CurrentLevel i;
    public void Write(CurrentLevel value) => i = value;// YandexGame.savesData.CurrentLevelIndex = value;

    private bool ContainsLevelIndex(int index, out int levelInfoIndex)
    {
        levelInfoIndex = -1;

        //var levelInfos = YandexGame.savesData.LevelInfo;

        //for (int i = 0; i < levelInfos.Length; i++)
        //{
        //    if (levelInfos[i].LevelIndex == index)
        //    {
        //        levelInfoIndex = i;
        //        return true;
        //    }
        //}

        return false;
    }

    private void AddLevelInfo(LevelInfo value)
    {
        //var levelInfo = YandexGame.savesData.LevelInfo;
        //var newLevelInfo = new LevelInfo[levelInfo.Length + 1];

        //for (int i = 0; i < levelInfo.Length; i++)
        //{
        //    newLevelInfo[i] = levelInfo[i];
        //}

        //newLevelInfo[newLevelInfo.Length - 1] = value;

        //YandexGame.savesData.LevelInfo = newLevelInfo;
    }

    private void UpdateLevelInfo(LevelInfo value, int index)
    {
        //YandexGame.savesData.LevelInfo[index] = value;
    }
}