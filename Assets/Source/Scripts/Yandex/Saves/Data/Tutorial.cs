[System.Serializable]
public class Tutorial : IPlayerData
{
    [UnityEngine.SerializeField] private bool _isNotFirstCession;

    public bool Menu => _isNotFirstCession;

    public Tutorial(bool isNotFirstCession) =>
        _isNotFirstCession = isNotFirstCession;
}