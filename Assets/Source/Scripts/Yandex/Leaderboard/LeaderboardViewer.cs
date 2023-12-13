using System.Collections;
using Agava.YandexGames;
using UnityEngine;

namespace BikeDefied.Yandex.Leaders
{
    [RequireComponent(typeof(LeaderboardModel))]
    public class LeaderboardViewer : MonoBehaviour
    {
        [SerializeField] private int _countVisiblePlayers;
        [SerializeField] private bool _isAuthorizedEmulation;
        [SerializeField] private bool _isHideIfNotAuthorized;

        private Coroutine _showCoroutine;

        private LeaderboardModel _model;

        private void Awake() =>
            _model = GetComponent<LeaderboardModel>();

        private void OnEnable() =>
            Show();

        private void Show()
        {
            bool isAuthorized = _isAuthorizedEmulation;
#if !UNITY_EDITOR
            isAuthorized = PlayerAccount.IsAuthorized;
#endif

            if (isAuthorized)
            {
#if !UNITY_EDITOR
                if (!PlayerAccount.HasPersonalProfileDataPermission)
                {
                    PlayerAccount.RequestPersonalProfileDataPermission();
                }
#endif
                if (_showCoroutine != null)
                {
                    StopCoroutine(_showCoroutine);
                }

                _showCoroutine = StartCoroutine(ShowLeaderboard());
            }
            else if (_isHideIfNotAuthorized)
            {
                gameObject.SetActive(false);
            }
            else
            {
                ShowBestScore();
            }
        }

        private void ShowBestScore()
        {
            var data = _model.GetPlayerData();

            CreatePlayerDataInTable(new LeaderboardPlayerData[] { data });
        }

        private IEnumerator ShowLeaderboard()
        {
            yield return StartCoroutine(_model.UpdateEntries());
            yield return StartCoroutine(_model.UpdatePlayerEntry());

            int count = _countVisiblePlayers == 0
                            ? _model.CountPlayers
                            : Mathf.Min(_countVisiblePlayers, _model.CountPlayers);

            CreatePlayerDataInTable(_model.GetPlayersData(count));
        }

        private void CreatePlayerDataInTable(LeaderboardPlayerData[] datas)
        {
            foreach (var data in datas)
            {
                var playerData = _model.Create(data);
                playerData.SelfGameObject.transform.localScale = Vector3.one;
            }
        }
    }
}