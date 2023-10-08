using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

public class UnmuteControlToggle : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;

    private ISaver _saver;
    private IAudioController _controller;

    private void OnEnable()
    {
        if (_saver != null)
        {
            _toggle.isOn = _saver.Get<UnmuteSound>().VolumePercent == 0 ? false : true;
            OnToggleChanged(_toggle.isOn);
        }
    }

    [Inject]
    private void Inject(ISaver saver, IAudioController audioController)
    {
        _saver = saver;
        _controller = audioController;
        _toggle.isOn = _saver.Get<UnmuteSound>().VolumePercent == 0 ? false : true;
        OnToggleChanged(_toggle.isOn);
    }

    public void OnToggleChanged(bool value)
    {
        float volume = value ? 1 : 0;
        _controller.VolumePercent = volume;
        _saver.Set(new UnmuteSound(volume));
    }
}