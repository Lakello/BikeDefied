[System.Serializable]
public class UnmuteSound : IPlayerData
{
    [UnityEngine.SerializeField] private float _volumePercent;

    public float VolumePercent => _volumePercent;

    public UnmuteSound(float volumePercent) =>
        _volumePercent = volumePercent;
}