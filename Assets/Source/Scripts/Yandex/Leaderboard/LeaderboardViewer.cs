using Agava.YandexGames;
using IJunior.Object;
using Reflex.Attributes;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardViewer : MonoBehaviour
{
    [SerializeField] private LeaderboardPlayerDataHandler _playerDataPrefab;
    [SerializeField] private Transform _content;
    [SerializeField] private int _countPlayers;
    [SerializeField] private Sprite _firstPlayerIcon;
    [SerializeField] private Sprite _secondPlayerIcon;
    [SerializeField] private Sprite _otherPlayerIcon;
    [SerializeField] private bool _isAuthorizedSim;
    [SerializeField] private bool _isHideIfNotAuthorized;

    private ObjectSpawner<LeaderboardPlayerData> _playerDataSpawner;
    private ISaver _saver;
    private YandexSimulator _yandexSimulator = new();
    private Coroutine _showCoroutine;

    private LeaderboardEntryResponse[] _allPlayers;
    private LeaderboardEntryResponse _playerEntry;

    private void Awake() =>
        _playerDataSpawner = new ObjectSpawner<LeaderboardPlayerData>(new ObjectFactory<LeaderboardPlayerData>(_content),
                                                                      new ObjectPool<LeaderboardPlayerData>());

    private void OnEnable() =>
        Show();

    private void OnDisable() =>
        _saver?.UnsubscribeValueUpdated<LevelInfo>(SetScore);

    [Inject]
    private void Inject(ISaver saver)
    {
        _saver = saver;
        saver.SubscribeValueUpdated<LevelInfo>(SetScore);
    }

    private void Show()
    {
        bool isAuthorized = _isAuthorizedSim;
#if !UNITY_EDITOR
        isAuthorized = PlayerAccount.IsAuthorized;
#endif

        if (isAuthorized)
        {
            if (_showCoroutine != null)
                StopCoroutine(_showCoroutine);

            _showCoroutine = StartCoroutine(ShowLeaderboard());
        }
        else if(_isHideIfNotAuthorized)
            gameObject.SetActive(false);
        else
            ShowBestScore();
    }

    private void ShowBestScore()
    {
        int index = _saver.Get<CurrentLevel>().Index;
        int score = _saver.Get(new LevelInfo(index, 0)).BestScore;
        var data = GetPlayerData(rank: 1, name: GetLocalizationAnonymousName(), score: score);

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

            if (string.IsNullOrEmpty(name))
                name = GetLocalizationAnonymousName();

            CreatePlayerDataInTable(GetPlayerData(rank, name, score));
        }
    }

    private LeaderboardPlayerData GetPlayerData(int rank, string name, int score)
    {
        int index = rank - 1;

        return new LeaderboardPlayerData
        {
            Rank = rank.ToString(),
            Name = name,
            Score = score.ToString(),

            Avatar = index == 0 ? _firstPlayerIcon
                                : index == 1 ? _secondPlayerIcon
                                : _otherPlayerIcon
        };
    }

    private void CreatePlayerDataInTable(LeaderboardPlayerData data)
    {
        var playerData = _playerDataSpawner.Spawn(_playerDataPrefab);
        playerData.Init(data);
        playerData.SelfGameObject.transform.localScale = Vector3.one;
    }

    private void SetScore(LevelInfo levelInfo)
    {
#if !UNITY_EDITOR
        int levelIndex = _saver.Get<CurrentLevel>().Index;

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

    private string GetLocalizationAnonymousName()
    {
        return GameLanguage.Value switch
        {
            "ru" => "Анонимный",
            "en" => "Anonymous",
            "tr" => "Anonim",
            _ => string.Empty,
        };
    }

    private string GetLeaderboardName()
    {
        string leaderboardName = $"Level{_saver.Get<CurrentLevel>().Index + 1}";
        return leaderboardName;
    }
}