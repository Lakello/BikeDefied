using System;
using UnityEngine;

public class Context : MonoBehaviour
{
    public event Action<bool> FocusChanged;

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
            Time.timeScale = 1.0f;
        else
            Time.timeScale = 0.0f;

        FocusChanged?.Invoke(focus);
    }
}