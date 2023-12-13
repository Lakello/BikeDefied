namespace BikeDefied.AudioSystem
{
    public interface IAudioVolumeChanger
    {
        public float VolumePercent { get; set; }
        
        public void Play(AudioType audio);
    }
}