using System;
using BikeDefied.Game.Spawner;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BikeDefied.Yandex.Leaders
{
    public class LeaderboardPlayerDataHandler : MonoBehaviour, IPoolingObject<LeaderboardPlayerData>
    {
        [SerializeField] private TMP_Text _rank;
        [SerializeField] private Image _avatar;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _score;
        
        public event Action<IPoolingObject<LeaderboardPlayerData>> Disabled;
        
        public Type SelfType => GetType();
        
        public GameObject SelfGameObject => gameObject;
        
        private void OnDisable() =>
            Disabled?.Invoke(this);

        public void Init(LeaderboardPlayerData data)
        {
            _rank.text = data.Rank;
            _avatar.sprite = data.Avatar;
            _name.text = data.Name;
            _score.text = data.Score;
        }
    }
}