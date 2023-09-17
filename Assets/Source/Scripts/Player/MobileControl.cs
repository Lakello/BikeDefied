using System;
using UnityEngine;

public class MobileControl : MonoBehaviour
{
    public event Action<int> HorizontalChanged;

    public void OnLeftDown() =>
        HorizontalChanged?.Invoke(-1);

    public void OnButtonUp() =>
        HorizontalChanged?.Invoke(0);

    public void OnRightDown() =>
        HorizontalChanged?.Invoke(1);
}