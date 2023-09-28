using Reflex.Attributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintViewer : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private List<Hint> _hints;

    private ISaver _saver;

    [Inject]
    private void Inject(ISaver saver)
    {
        _saver = saver;
        _toggle.isOn = _saver.Get<HintDisplay>().IsHintDisplay;

        foreach (var hint in _hints)
            hint.StartShow(_toggle.isOn);
    }

    public void OnToggleChanged(bool value)
    {
        _saver.Set(new HintDisplay(value));

        foreach (var hint in _hints)
            hint.HindDisplayUpdated(value);
    }
}