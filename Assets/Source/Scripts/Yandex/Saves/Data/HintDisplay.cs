namespace BikeDefied.Yandex.Saves.Data
{
    [System.Serializable]
    public class HintDisplay : IPlayerData
    {
        [UnityEngine.SerializeField] private bool _isHintDisplay;
        
        public HintDisplay(bool isHintDisplay) =>
            _isHintDisplay = isHintDisplay;
        
        public bool IsHintDisplay => _isHintDisplay;
    }
}