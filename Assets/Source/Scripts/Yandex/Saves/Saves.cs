using Agava.YandexGames;
using System;
using System.Linq;
using UnityEngine;

public class Saves : ISaver<CurrentLevel>, ISaverArray<LevelInfo>, IDisposable
{
    private PlayerData _playerData;
    private IGameOver _over;

    [Serializable]
    private class PlayerData
    {
        public LevelInfo[] LevelInfo = new LevelInfo[] { };
        public CurrentLevel CurrentLevel = new CurrentLevel();
    }

    public Saves(IGameOver over)
    {
        _over = over;
        _over.LateGameOver += Save;

        _playerData = new PlayerData();
    }

    public CurrentLevel Get() =>
        _playerData.CurrentLevel;

    public void Set(CurrentLevel value) =>
        SetCurrentLevel(value);

    public LevelInfo Get(int index) =>
        _playerData.LevelInfo.FirstOrDefault(levelInfo => levelInfo.LevelIndex == index);

    public void Set(LevelInfo value) =>
        SetLevelInfo(value);

    public void Init()
    {
        Debug.Log("SAVE INIT");

#if !UNITY_EDITOR
        PlayerAccount.GetCloudSaveData((data) =>
        {
            var playerData = JsonUtility.FromJson<PlayerData>(data);

            if (playerData.LevelInfo != null && playerData.LevelInfo.Length > 0)
            {
                foreach (var levelInfo in playerData.LevelInfo)
                {
                    SetLevelInfo(levelInfo);
                }

                SetCurrentLevel(playerData.CurrentLevel);
            }

            Debug.Log($"data = {data}");
        });
#endif
    }

    public void Dispose()
    {
        _over.LateGameOver -= Save;
    }

    private void Save()
    {
        string strData = "";
        PlayerAccount.GetCloudSaveData((data) => strData = data);
        string s = JsonUtility.ToJson(_playerData);
        Debug.Log($"SAVE = {s}");
#if !UNITY_EDITOR
        if (strData != s)
            PlayerAccount.SetCloudSaveData(s);
#endif
    }

    private void SetCurrentLevel(CurrentLevel value) => _playerData.CurrentLevel = value;

    private void SetLevelInfo(LevelInfo levelInfo)
    {
        if (ContainsLevelInfo(levelInfo, out int index))
            UpdateLevelInfo(levelInfo, index);
        else
            AddLevelInfo(levelInfo);
    }

    private void UpdateLevelInfo(LevelInfo value, int index)
    {
        if (_playerData.LevelInfo[index].BestScore < value.BestScore)
            _playerData.LevelInfo[index] = value;
    }

    private bool ContainsLevelInfo(LevelInfo levelInfo, out int index)
    {
        index = -1;

        for (int i = 0; i < _playerData.LevelInfo.Length; i++)
        {
            if (_playerData.LevelInfo[i].LevelIndex == levelInfo.LevelIndex)
            {
                index = i;
                return true;
            }
        }

        return false;
    }

    private void AddLevelInfo(LevelInfo value)
    {
        var newLevelInfo = new LevelInfo[_playerData.LevelInfo.Length + 1];

        for (int i = 0; i < _playerData.LevelInfo.Length; i++)
        {
            newLevelInfo[i] = _playerData.LevelInfo[i];
        }

        newLevelInfo[^1] = value;

        _playerData.LevelInfo = newLevelInfo;
    }
}