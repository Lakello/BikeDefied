namespace BikeDefied.AudioSystem
{
    public interface IAudioController
    {
        public void Play(AudioType audio);
        public float VolumePercent { get; set; }
    }
}