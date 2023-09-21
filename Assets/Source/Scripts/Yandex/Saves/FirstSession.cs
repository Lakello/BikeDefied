[System.Serializable]
public class FirstSession : IPlayerData
{
    [UnityEngine.SerializeField] private bool _isFirstCession;

    public bool IsFirstCession => _isFirstCession;

    public FirstSession(bool isFirstCession) =>
        _isFirstCession = isFirstCession;
}