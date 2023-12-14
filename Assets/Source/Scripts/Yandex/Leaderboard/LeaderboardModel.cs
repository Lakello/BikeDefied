using System;
using System.Collections;
using System.Collections.Generic;
using Agava.YandexGames;
using BikeDefied.Game.Spawner;
using BikeDefied.Yandex.Emulator;
using BikeDefied.Yandex.Localization;
using BikeDefied.Yandex.Saves;
using BikeDefied.Yandex.Saves.Data;
using Reflex.Attributes;
using UnityEngine;

namespace BikeDefied.Yandex.Leaders
{
    public class LeaderboardModel : MonoBehaviour
    {
        private readonly int _defaultRank = 1;
        private readonly YandexEmulator _yandexEmulator = new YandexEmulator();

        [SerializeField] private Transform _content;
        [SerializeField] private LeaderboardPlayerDataHandler _playerDataPrefab;
        [SerializeField] private Sprite _firstPlayerIcon;
        [SerializeField] private Sprite _secondPlayerIcon;
        [SerializeField] private Sprite _otherPlayerIcon;

        private ISaver _saver;
        private ObjectSpawner<LeaderboardPlayerData> _playerDataSpawner;
        private Dictionary<PlayerIconType, Sprite> _playerIcons;

        private LeaderboardEntryResponse[] _allPlayers;
        private LeaderboardEntryResponse _playerEntry;

        public int CountPlayers => _allPlayers.Length;

        private int GetCurrentLevelScore => _saver.Get(new LevelInfo(_saver.Get<CurrentLevel>().Index, 0)).BestScore;

        [Inject]
        private void Inject(ISaver saver)
        {
            _saver = saver;
            saver.SubscribeValueUpdated<LevelInfo>(SetScore);

            _playerDataSpawner = new ObjectSpawner<LeaderboardPlayerData>(
                new ObjectFactory<LeaderboardPlayerData>(_content),
                new ObjectPool<LeaderboardPlayerData>());

            _playerIcons = new Dictionary<PlayerIconType, Sprite>
            {
                [PlayerIconType.First] = _firstPlayerIcon, [PlayerIconType.Second] = _secondPlayerIcon, [PlayerIconType.Other] = _otherPlayerIcon,
            };
        }

        private void OnDisable() =>
            _saver?.UnsubscribeValueUpdated<LevelInfo>(SetScore);

        public IPoolingObject<LeaderboardPlayerData> Create(LeaderboardPlayerData data) =>
            _playerDataSpawner.Spawn(_playerDataPrefab, data);

        public LeaderboardPlayerData GetPlayerData() =>
            GetPlayerData(_defaultRank, GetLocalizationAnonymousName(), GetCurrentLevelScore);

        public LeaderboardPlayerData[] GetPlayersData(int count)
        {
            LeaderboardPlayerData[] data = new LeaderboardPlayerData[count];
            Action addPlayer = null;

            if (_playerEntry.rank > count)
            {
                addPlayer = () => data[^1] = GetPlayerData(_playerEntry.rank, _playerEntry.player.publicName, _playerEntry.score);
                count--;
            }

            for (int i = 0; i < count; i++)
            {
                string publicName = _allPlayers[i].player.publicName;

                if (string.IsNullOrEmpty(publicName))
                {
                    publicName = GetLocalizationAnonymousName();
                }

                data[i] = GetPlayerData(_allPlayers[i].rank, publicName, _allPlayers[i].score);
            }

            addPlayer?.Invoke();

            return data;
        }

        public IEnumerator UpdateEntries()
        {
            bool isSuccess = false;

#if !UNITY_EDITOR
            Leaderboard.GetEntries(
                GetLeaderboardName(),
                (result) =>
                {
                    _allPlayers = result.entries;
                    isSuccess = true;
                });
#else
            _allPlayers = _yandexEmulator.GetLeaderboardAllPlayers();
            isSuccess = true;
#endif

            while (!isSuccess)
            {
                yield return null;
            }
        }

        public IEnumerator UpdatePlayerEntry()
        {
            bool isSuccess = false;

#if !UNITY_EDITOR
            Leaderboard.GetPlayerEntry(
                GetLeaderboardName(),
                (result) =>
                {
                    _playerEntry = result;
                    isSuccess = true;
                });
#else
            _playerEntry = _yandexEmulator.GetLeaderboardPlayerEntry();
            isSuccess = true;
#endif

            while (!isSuccess)
            {
                yield return null;
            }
        }

        public string GetLocalizationAnonymousName()
        {
            return GameLanguage.Value switch
            {
                "ru" => "Анонимный",
                "en" => "Anonymous",
                "tr" => "Anonim",
                _ => string.Empty,
            };
        }

        private LeaderboardPlayerData GetPlayerData(int rank, string name, int score)
        {
            int iconIndex = rank - 1;
            iconIndex = Mathf.Clamp(iconIndex, 0, Enum.GetNames(typeof(PlayerIconType)).Length - 1);

            if (!_playerIcons.TryGetValue((PlayerIconType)iconIndex, out Sprite avatar))
            {
                throw new NullReferenceException();
            }

            return new LeaderboardPlayerData
            {
                Rank = rank.ToString(), Name = name, Score = score.ToString(), Avatar = avatar,
            };
        }

        private string GetLeaderboardName() =>
            $"Level{_saver.Get<CurrentLevel>().Index + 1}";

        private void SetScore(LevelInfo levelInfo)
        {
#if !UNITY_EDITOR
            int levelIndex = _saver.Get<CurrentLevel>().Index;

            int score = levelInfo.BestScore;
            if (PlayerAccount.IsAuthorized)
            {
                Leaderboard.SetScore(GetLeaderboardName(), score);
            }
#endif
        }
    }
}