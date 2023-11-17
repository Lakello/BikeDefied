using System;
using System.IO;
using Agava.YandexGames;

namespace BikeDefied.Yandex.Emulator
{
    public class YandexEmulator
    {
        private readonly string _saveSimPath = "Assets/Source/Scripts/Yandex/Emulator/SaveSim.json";
        private readonly int _playerRank = 2;
        private LeaderboardEntryResponse _playerEntrySim;
        private LeaderboardEntryResponse[] _allPlayersSim;

        public void Init(Action<string> action)
        {
            string data = File.ReadAllText(_saveSimPath);

            action?.Invoke(data);
        }

        public void Save(string save) =>
            File.WriteAllText(_saveSimPath, save);

        public LeaderboardEntryResponse[] GetLeaderboardAllPlayers()
        {
            int rank = 0;

            LeaderboardEntryResponse GetEntry()
            {
                LeaderboardEntryResponse entry = new LeaderboardEntryResponse();
                PlayerAccountProfileDataResponse player = new PlayerAccountProfileDataResponse();

                entry.rank = ++rank == _playerRank ? ++rank : rank;
                entry.player = player;
                entry.player.publicName = "anoano";
                entry.score = UnityEngine.Random.Range(100, 1000);

                return entry;
            }

            int count = 19;
            _allPlayersSim = new LeaderboardEntryResponse[count];

            for (int i = 0; i < count; i++)
            {
                if (i + 1 == _playerRank)
                {
                    _allPlayersSim[i] = GetLeaderboardPlayerEntry();
                }
                else
                {
                    _allPlayersSim[i] = GetEntry();
                }
            }

            return _allPlayersSim;
        }

        public LeaderboardEntryResponse GetLeaderboardPlayerEntry()
        {
            _playerEntrySim = new LeaderboardEntryResponse();
            PlayerAccountProfileDataResponse player = new PlayerAccountProfileDataResponse();

            _playerEntrySim.rank = _playerRank;
            _playerEntrySim.player = player;
            _playerEntrySim.player.publicName = "player";
            _playerEntrySim.score = UnityEngine.Random.Range(100, 1000);

            return _playerEntrySim;
        }
    }
}