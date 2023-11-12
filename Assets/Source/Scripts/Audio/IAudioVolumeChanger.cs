namespace BikeDefied.AudioSystem
{
    public interface IAudioVolumeChanger
    {
        public void Play(AudioType audio);
        public float VolumePercent { get; set; }
    }
}