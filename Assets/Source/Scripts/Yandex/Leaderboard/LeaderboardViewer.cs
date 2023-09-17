using Reflex.Attributes;
using System;
using System.Collections;
using UnityEngine;
using Agava.YandexGames;
using System.Collections.Generic;
using IJunior.Object;

public class LeaderboardViewer : MonoBehaviour
{
    [SerializeField] private LeaderboardPlayerData _playerDataPrefab;
    [SerializeField] private Transform _content;
    [SerializeField] private int _countPlayers;
    [SerializeField] private Sprite _firstPlayerIcon;
    [SerializeField] private Sprite _secondPlayerIcon;
    [SerializeField] private Sprite _otherPlayerIcon;

    private Func<CurrentLevel> _getCurrentLevel;
    private ObjectSpawner<PlayerData> _playerDataSpawner;
    private ISaverArray<LevelInfo> _levelInfo;
    private YandexSimulator _yandexSimulator = new();
    private Coroutine _showCoroutine;
    private LeaderboardEntryResponse[] _allPlayers;
    private LeaderboardEntryResponse _playerEntry;

    private void OnEnable() =>
        Show();

    private void Awake() =>
        _playerDataSpawner = new ObjectSpawner<PlayerData>(new ObjectFactory<PlayerData>(_content), new ObjectPool<PlayerData>());

    private void OnDisable()
    {
        if (_levelInfo != null)
            _levelInfo.ValueUpdated -= SetScore;
    }

    [Inject]
    private void Inject(ISaver<CurrentLevel> currentLevel, ISaverArray<LevelInfo> levelInfo)
    {
        _getCurrentLevel = () => currentLevel.Get();

        _levelInfo = levelInfo;
        _levelInfo.ValueUpdated += SetScore;
    }

    private void Show()
    {
        if (PlayerAccount.IsAuthorized)
        {
            if (_showCoroutine != null)
                StopCoroutine(_showCoroutine);

            _showCoroutine = StartCoroutine(ShowLeaderboard());
        }
        else
            ShowBestScore();
    }

    private void ShowBestScore()
    {
        Debug.Log("GET INDEX");
        int index = _getCurrentLevel().Index;
        Debug.Log("GET SCORE");
        int score = _levelInfo.Get(index).BestScore;
        Debug.Log("GET PLAYER DATA");
        var data = GetPlayerData(rank: 1, name: "Player", score: score);

        CreatePlayerDataInTable(data);
    }

    private IEnumerator ShowLeaderboard()
    {
        yield return StartCoroutine(UpdateEntries());
        yield return StartCoroutine(UpdatePlayerEntry());

        int count = _countPlayers == 0 ? _allPlayers.Length
                        : _countPlayers > _allPlayers.Length ? _allPlayers.Length
                        : _countPlayers;

        bool playerSpawned = false;

        int rank;
        string name;
        int score;

        for (int i = 0; i < count; i++)
        {
            if (i == _playerEntry.rank - 1)
                playerSpawned = true;

            (rank, name, score) = (i == count - 1 && !playerSpawned && _countPlayers != 0)
                                    ? (_playerEntry.rank, _playerEntry.player.publicName, _playerEntry.score)
                                    : (_allPlayers[i].rank, _allPlayers[i].player.publicName, _allPlayers[i].score);

            CreatePlayerDataInTable(GetPlayerData(rank, name, score));
        }
    }

    private PlayerData GetPlayerData(int rank, string name, int score)
    {
        int index = rank - 1;

        return new PlayerData
        {
            Rank = rank.ToString(),
            Name = name,
            Score = score.ToString(),

            Avatar = index == 0 ? _firstPlayerIcon
                                : index == 1 ? _secondPlayerIcon
                                : _otherPlayerIcon
        };
    }

    private void CreatePlayerDataInTable(PlayerData data)
    {
        Debug.Log("ENTER CREATE");

        var playerData = _playerDataSpawner.Spawn(_playerDataPrefab);
        Debug.Log("INIT");
        playerData.Init(data);
        playerData.SelfGameObject.transform.localScale = Vector3.one;
        Debug.Log("CREATE");
    }

    private void SetScore(LevelInfo levelInfo)
    {
#if !UNITY_EDITOR
        int levelIndex = _getCurrentLevel().Index;

        int score = levelInfo.BestScore;
        if (PlayerAccount.IsAuthorized)
            Leaderboard.SetScore(GetLeaderboardName(), score);
#endif
    }

    private IEnumerator UpdateEntries()
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
        _allPlayers = _yandexSimulator.GetLeaderboardAllPlayers();
        isSuccess = true;
#endif

        while (!isSuccess)
            yield return null;
    }

    private IEnumerator UpdatePlayerEntry()
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
        _playerEntry = _yandexSimulator.GetLeaderboardPlayerEntry();
        isSuccess = true;
#endif

        while (!isSuccess)
            yield return null;
    }

    private string GetLeaderboardName()
    {
        string leaderboardName = $"Level{_getCurrentLevel().Index + 1}";
        return leaderboardName;
    }
}