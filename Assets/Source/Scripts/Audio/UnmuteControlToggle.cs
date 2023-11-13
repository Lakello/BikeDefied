using BikeDefied.Yandex.Saves;
using BikeDefied.Yandex.Saves.Data;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace BikeDefied.AudioSystem
{
    public class UnmuteControlToggle : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;

        private ISaver _saver;
        private IAudioVolumeChanger _volumeChanger;

        [Inject]
        private void Inject(ISaver saver, IAudioVolumeChanger audioVolumeChanger)
        {
            _saver = saver;
            _volumeChanger = audioVolumeChanger;
            _toggle.isOn = _saver.Get<UnmuteSound>().VolumePercent == 0 ? false : true;
        }

        public void OnToggleChanged(bool value)
        {
            float volume = value ? 1 : 0;
            _volumeChanger.VolumePercent = volume;
            _saver.Set(new UnmuteSound(volume));
        }
    }
}