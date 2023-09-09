using Agava.YandexGames;
using Reflex.Attributes;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class Saves : IReadFromArray<LevelInfo>, IRead<CurrentLevel>, IWrite<LevelInfo>, IWrite<CurrentLevel>, IDisposable
{
    private PlayerData _playerData;
    private IGameOver _over;

    [Serializable]
    protected struct PlayerData
    {
        public LevelInfo[] Levelinfo;
        public CurrentLevel CurrentLevel;
    }

    public Saves(IGameOver over)
    {
        _over = over;
        _over.GameOver += Save;

        _playerData = new PlayerData();
        _playerData.CurrentLevel.Index = 0;

        var levelInfo = new LevelInfo[7];

        for (int i = 0; i < levelInfo.Length; i++)
        {
            levelInfo[i].LevelIndex = i;
            levelInfo[i].BestScore = 1;
        }

        _playerData.Levelinfo = levelInfo;
    }

    public void Init()
    {
#if !UNITY_EDITOR
        PlayerAccount.GetCloudSaveData((data) =>
        {
            var playerData = JsonUtility.FromJson<PlayerData>(data);

            if (playerData.Levelinfo != null && playerData.Levelinfo.Length > 0)
            {
                for (int i = 0; i < playerData.Levelinfo.Length; i++)
                {
                    _playerData.Levelinfo[i] = playerData.Levelinfo[i];
                }

                _playerData.CurrentLevel = playerData.CurrentLevel;
            }

            Debug.Log($"data = {data}");
        });
#endif
    }

    public void Dispose()
    {
        _over.GameOver -= Save;
    }

    public LevelInfo Read(int index) => _playerData.Levelinfo[index];

    public CurrentLevel Read() => _playerData.CurrentLevel;

    public void Write(LevelInfo value)
    {
        if (ContainsLevelIndex(value.LevelIndex, out int index))
            UpdateLevelInfo(value, index);
        else
            AddLevelInfo(value);
    }

    public void Write(CurrentLevel value) => _playerData.CurrentLevel = value;

    private void Save()
    {
        var s = JsonUtility.ToJson(_playerData);
        Debug.Log($"SAVE = {s}");
#if !UNITY_EDITOR
        PlayerAccount.SetCloudSaveData(s);
#endif
    }

    private bool ContainsLevelIndex(int index, out int levelInfoIndex)
    {
        levelInfoIndex = -1;

        for (int i = 0; i < _playerData.Levelinfo.Length; i++)
        {
            if (_playerData.Levelinfo[i].LevelIndex == index)
            {
                levelInfoIndex = i;
                return true;
            }
        }

        return false;
    }

    private void AddLevelInfo(LevelInfo value)
    {
        var newLevelInfo = new LevelInfo[_playerData.Levelinfo.Length + 1];

        for (int i = 0; i < _playerData.Levelinfo.Length; i++)
        {
            newLevelInfo[i] = _playerData.Levelinfo[i];
        }

        newLevelInfo[newLevelInfo.Length - 1] = value;

        _playerData.Levelinfo = newLevelInfo;
    }

    private void UpdateLevelInfo(LevelInfo value, int index)
    {
        _playerData.Levelinfo[index] = value;
    }
}