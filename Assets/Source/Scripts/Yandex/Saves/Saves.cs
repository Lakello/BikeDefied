using Agava.YandexGames;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;

public class Saves : ISaver<CurrentLevel>, ISaverArray<LevelInfo>
{
    private PlayerData _playerData = new();
    private YandexSimulator _yandexSimulator = new();

    event Action<CurrentLevel> ISaver<CurrentLevel>.ValueUpdated
    { 
        add
        { 
            _currentLevelUpdated += value;
        }
        remove
        {
            _currentLevelUpdated -= value;
        }
    }

    event Action<LevelInfo> ISaverArray<LevelInfo>.ValueUpdated
    {
        add
        {
            _levelInfoUpdated += value;
        }
        remove
        {
            _levelInfoUpdated -= value;
        }
    }

    private Action<CurrentLevel> _currentLevelUpdated;
    private Action<LevelInfo> _levelInfoUpdated;

    [Serializable]
    private class PlayerData
    {
        public LevelInfo[] LevelInfo = new LevelInfo[] { };
        public CurrentLevel CurrentLevel = new CurrentLevel(0);
    }

    public CurrentLevel Get() =>
        _playerData.CurrentLevel;

    public void Set(CurrentLevel value)
    {
        if (_playerData.CurrentLevel != value)
        {
            _playerData.CurrentLevel = value;
            Save(_currentLevelUpdated, value);
        }
    }

    public LevelInfo Get(int index) =>
        _playerData.LevelInfo.FirstOrDefault(levelInfo => levelInfo.LevelIndex == index);

    public void Set(LevelInfo value)
    {
        bool isSetted = true;

        if (ContainsLevelInfo(value, out int index))
        {
            if (!TryUpdateLevelInfo(value, index))
                isSetted = false;
        }
        else
            AddLevelInfo(value);

        if (isSetted)
            Save(_levelInfoUpdated, value);
    }

    public void Init()
    {
#if !UNITY_EDITOR
        PlayerAccount.GetCloudSaveData((data) =>
        {
            var playerData = JsonUtility.FromJson<PlayerData>(data);

            if (playerData.LevelInfo != null && playerData.LevelInfo.Length > 0)
            {
                foreach (var levelInfo in playerData.LevelInfo)
                {
                    AddLevelInfo(levelInfo);
                }

                Set(playerData.CurrentLevel);
            }

            Debug.Log($"data = {data}");
        });
#else
        _yandexSimulator.Init((data) =>
        {
            var playerData = JsonUtility.FromJson<PlayerData>(data);

            if (playerData.LevelInfo != null && playerData.LevelInfo.Length > 0)
            {
                foreach (var levelInfo in playerData.LevelInfo)
                {
                    AddLevelInfo(levelInfo);
                }

                Set(playerData.CurrentLevel);
            }
        });
#endif
    }

    private void Save<T>(Action<T> saved, T valueCallback)
    {
        string save = JsonUtility.ToJson(_playerData);
#if !UNITY_EDITOR
        PlayerAccount.SetCloudSaveData(save);
#else
        _yandexSimulator.Save(save);
#endif
        saved?.Invoke(valueCallback);
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

    private bool TryUpdateLevelInfo(LevelInfo value, int index)
    {
        if (_playerData.LevelInfo[index].BestScore < value.BestScore)
        {
            _playerData.LevelInfo[index] = value;
            return true;
        }

        return false;
    }

    private void AddLevelInfo(LevelInfo value)
    {
        var newLevelInfo = new LevelInfo[_playerData.LevelInfo.Length + 1];

        for (int i = 0; i < _playerData.LevelInfo.Length; i++)
            newLevelInfo[i] = _playerData.LevelInfo[i];

        newLevelInfo[^1] = value;

        _playerData.LevelInfo = newLevelInfo;
    }
}