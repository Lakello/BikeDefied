using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardPlayerData : MonoBehaviour, IPoolingObject<PlayerData>
{
    [SerializeField] private TMP_Text _rank;
    [SerializeField] private Image _avatar;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _score;

    public Type SelfType => GetType();
    public GameObject SelfGameObject => gameObject;

    public event Action<IPoolingObject<PlayerData>> Disable;

    private void OnDisable() =>
        Disable?.Invoke(this);

    public void Init(PlayerData data)
    {
        Debug.Log($"DATA PARENT = {transform.parent.name}");
        _rank.text = data.Rank;
        _avatar.sprite = data.Avatar;
        _name.text = data.Name;
        _score.text = data.Score;
    }
}