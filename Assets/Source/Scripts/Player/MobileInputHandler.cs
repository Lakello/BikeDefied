using Reflex.Attributes;
using UnityEngine;

public class MobileInputHandler : MonoBehaviour, IInputHandler
{
    private int _input;
    private MobileControl _control;

    public float Horizontal => _input;

    private void OnDisable()
    {
        if (_control != null)
            _control.HorizontalChanged -= OnHorizontalChanged;
    }

    public void Init(MobileControl control)
    {
        _control = control;
        _control.HorizontalChanged += OnHorizontalChanged;
    }

    private void OnHorizontalChanged(int value) =>
        _input = Mathf.Clamp(value, -1, 1);
}