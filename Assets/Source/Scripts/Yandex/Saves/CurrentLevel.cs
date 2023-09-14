[System.Serializable]
public class CurrentLevel
{
    [UnityEngine.SerializeField] private int _index;

    public int Index => _index;

    public CurrentLevel(int index) =>
        _index = index;
}