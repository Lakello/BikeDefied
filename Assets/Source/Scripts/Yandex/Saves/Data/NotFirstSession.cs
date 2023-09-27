[System.Serializable]
public class NotFirstSession : IPlayerData
{
    [UnityEngine.SerializeField] private bool _isNotFirstCession;

    public bool IsNotFirstSession => _isNotFirstCession;

    public NotFirstSession(bool isNotFirstCession) =>
        _isNotFirstCession = isNotFirstCession;
}