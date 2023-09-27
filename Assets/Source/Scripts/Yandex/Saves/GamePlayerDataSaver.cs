using Agava.YandexGames;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class GamePlayerDataSaver : ISaver
{
    private PlayerData _playerData = new();
    private YandexSimulator _yandexSimulator = new();
    private Hashtable _accessMethodsHolders;
    private Hashtable _playerDataEvents;

    private event Action<CurrentLevel> _currentLevelUpdated;
    private event Action<LevelInfo> _levelInfoUpdated;

    public GamePlayerDataSaver()
    {
        _playerDataEvents = new Hashtable()
        {
            [typeof(LevelInfo)] = _levelInfoUpdated,
            [typeof(CurrentLevel)] = _currentLevelUpdated
        };

        _accessMethodsHolders = new Hashtable()
        {
            [typeof(LevelInfo)] = new SaveAccessMethodsHolder<LevelInfo>(
                getter: (value) =>
                {
                     return _playerData.LevelInfo.FirstOrDefault(levelInfo => levelInfo.LevelIndex == value.LevelIndex);
                }, 
                setter: (value) =>
                {
                    if (value == default)
                        throw new ArgumentNullException(nameof(value));

                    bool isSetted = true;

                    if (ContainsLevelInfo(value, out int index))
                    {
                        if (!TryUpdateLevelInfo(value, index))
                            isSetted = false;
                    }
                    else
                        AddLevelInfo(value);

                    if (isSetted)
                        Save((Action<LevelInfo>)_playerDataEvents[typeof(LevelInfo)], value);
                }),

            [typeof(CurrentLevel)] = new SaveAccessMethodsHolder<CurrentLevel>(
                getter: (_) => _playerData.CurrentLevel,
                setter: (value) =>
                {
                    if (value == default)
                        throw new ArgumentNullException(nameof(value));

                    if (_playerData.CurrentLevel != value)
                    {
                        _playerData.CurrentLevel = value;
                        Save((Action<CurrentLevel>)_playerDataEvents[typeof(CurrentLevel)], value);
                    }
                }),

            [typeof(NotFirstSession)] = new SaveAccessMethodsHolder<NotFirstSession>(
                getter: (_) => _playerData.FirstSession,
                setter: (value) => _playerData.FirstSession = value ?? throw new ArgumentNullException(nameof(value)))
        };
    }

    [Serializable]
    private class PlayerData
    {
        public LevelInfo[] LevelInfo = new LevelInfo[] { };
        public CurrentLevel CurrentLevel = new(0);
        public NotFirstSession FirstSession = new(false);
    }

    public TData Get<TData>(TData value = default) where TData : class, IPlayerData
    {
        if (_accessMethodsHolders.ContainsKey(typeof(TData)))
        {
            var holder = (SaveAccessMethodsHolder<TData>)_accessMethodsHolders[typeof(TData)];
            return holder.Getter(value);
        }
        else
        {
            throw new ArgumentNullException($"{nameof(GamePlayerDataSaver)} GET DATA");
        }
    }

    public void Set<TData>(TData value = default) where TData : class, IPlayerData
    {
        if (_accessMethodsHolders.ContainsKey(typeof(TData)))
        {
            var holder = (SaveAccessMethodsHolder<TData>)_accessMethodsHolders[typeof(TData)];
            holder.Setter(value);
        }
        else
        {
            throw new ArgumentNullException($"{nameof(GamePlayerDataSaver)} SET DATA");
        }
    }

    public void SubscribeValueUpdated<TData>(Action<TData> subAction) where TData : class, IPlayerData
    {
        if (_playerDataEvents.ContainsKey(typeof(TData)))
        {
            var action = (Action<TData>)_playerDataEvents[typeof(TData)];

            action += subAction;
            _playerDataEvents[typeof(TData)] = action;
        }
        else
        {
            throw new ArgumentNullException($"{nameof(GamePlayerDataSaver)} VALUE UPDATED");
        }
    }

    public void UnsubscribeValueUpdated<TData>(Action<TData> subAction) where TData : class, IPlayerData
    {
        if (_playerDataEvents.ContainsKey(typeof(TData)))
        {
            var action = (Action<TData>)_playerDataEvents[typeof(TData)];

            action -= subAction;
            _playerDataEvents[typeof(TData)] = action;
        }
        else
        {
            throw new ArgumentNullException($"{nameof(GamePlayerDataSaver)} VALUE UPDATED");
        }
    }

    public void Init()
    {
        void onSuccessCallback(string data)
        {
            var playerData = JsonUtility.FromJson<PlayerData>(data);

            if (playerData.LevelInfo != null && playerData.LevelInfo.Length > 0)
            {
                foreach (var levelInfo in playerData.LevelInfo)
                {
                    AddLevelInfo(levelInfo);
                }

                Set(playerData.CurrentLevel);
                Set(playerData.FirstSession);
            }
        }

#if !UNITY_EDITOR
        PlayerAccount.GetCloudSaveData(onSuccessCallback);
#else
        _yandexSimulator.Init(onSuccessCallback);
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