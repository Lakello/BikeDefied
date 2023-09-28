[System.Serializable]
public class HintDisplay : IPlayerData
{
    [UnityEngine.SerializeField] private bool _isHintDisplay;

    public bool IsHintDisplay => _isHintDisplay;

    public HintDisplay(bool isHintDisplay) =>
        _isHintDisplay = isHintDisplay;
}