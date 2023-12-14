using System;
using System.Collections;
using System.Linq;
using Agava.YandexGames;
using BikeDefied.Yandex.Emulator;
using BikeDefied.Yandex.Saves.Data;
using UnityEngine;

namespace BikeDefied.Yandex.Saves
{
    public class GamePlayerDataSaver : ISaver
    {
        private PlayerData _playerData = new PlayerData();
        private YandexEmulator _yandexSimulator = new YandexEmulator();
        private Hashtable _accessMethodsHolders;
        private Hashtable _playerDataEvents;

        public GamePlayerDataSaver()
        {
            _playerDataEvents = new Hashtable();

            _accessMethodsHolders = new Hashtable()
            {
                [typeof(LevelInfo)] = new SaveAccessMethodsHolder<LevelInfo>(
                    getter: (value) =>
                        _playerData.LevelInfo.FirstOrDefault(levelInfo => levelInfo.LevelIndex == value.LevelIndex),
                    setter: (value) =>
                    {
                        if (value == default)
                        {
                            throw new ArgumentNullException(nameof(value));
                        }

                        bool isSetted = true;

                        if (ContainsLevelInfo(value, out int index))
                        {
                            if (!TryUpdateLevelInfo(value, index))
                            {
                                isSetted = false;
                            }
                        }
                        else
                        {
                            AddLevelInfo(value);
                        }

                        if (isSetted)
                        {
                            Save((Action<LevelInfo>)_playerDataEvents[typeof(LevelInfo)], value);
                        }
                    }),
                [typeof(CurrentLevel)] = new SaveAccessMethodsHolder<CurrentLevel>(
                    getter: (_) =>
                        _playerData.CurrentLevel,
                    setter: (value) =>
                    {
                        if (value == default)
                        {
                            throw new ArgumentNullException(nameof(value));
                        }

                        if (_playerData.CurrentLevel.Index != value.Index)
                        {
                            _playerData.CurrentLevel = value;
                            Save((Action<CurrentLevel>)_playerDataEvents[typeof(CurrentLevel)], value);
                        }
                    }),
                [typeof(HintDisplay)] = new SaveAccessMethodsHolder<HintDisplay>(
                    getter: (_) =>
                        _playerData.HintDisplay,
                    setter: (value) =>
                    {
                        if (_playerData.HintDisplay.IsHintDisplay != value.IsHintDisplay)
                        {
                            _playerData.HintDisplay = value;
                            Save();
                        }
                    }),
                [typeof(UnmuteSound)] = new SaveAccessMethodsHolder<UnmuteSound>(
                    getter: (_) =>
                        _playerData.UnmuteSound,
                    setter: (value) =>
                    {
                        if (_playerData.UnmuteSound.VolumePercent == value.VolumePercent)
                        {
                            return;
                        }

                        _playerData.UnmuteSound = value;
                        Save();
                    }),
            };
        }

        public TData Get<TData>(TData value = default)
            where TData : class, IPlayerData
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

        public void Set<TData>(TData value = default)
            where TData : class, IPlayerData
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

        public void SubscribeValueUpdated<TData>(Action<TData> subAction)
            where TData : class, IPlayerData
        {
            if (_playerDataEvents.ContainsKey(typeof(TData)))
            {
                var action = (Action<TData>)_playerDataEvents[typeof(TData)];

                action += subAction;
                _playerDataEvents[typeof(TData)] = action;
            }
            else
            {
                _playerDataEvents.Add(typeof(TData), subAction);
            }
        }

        public void UnsubscribeValueUpdated<TData>(Action<TData> subAction)
            where TData : class, IPlayerData
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
#if !UNITY_EDITOR
            PlayerAccount.GetCloudSaveData(OnSuccessCallback);
#else
            _yandexSimulator.Init(OnSuccessCallback);
#endif

            return;

            void OnSuccessCallback(string data)
            {
                var playerData = JsonUtility.FromJson<PlayerData>(data);

                foreach (var levelInfo in playerData.LevelInfo)
                {
                    AddLevelInfo(levelInfo);
                }

                Set(playerData.CurrentLevel);
                Set(playerData.HintDisplay);
                Set(playerData.UnmuteSound);
            }
        }

        private void Save()
        {
            string save = JsonUtility.ToJson(_playerData);
#if !UNITY_EDITOR
            PlayerAccount.SetCloudSaveData(save);
#else
            _yandexSimulator.Save(save);
#endif
        }

        private void Save<T>(Action<T> saved, T valueCallback)
        {
            Save();
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
            {
                newLevelInfo[i] = _playerData.LevelInfo[i];
            }

            newLevelInfo[^1] = value;

            _playerData.LevelInfo = newLevelInfo;
        }
    }
}