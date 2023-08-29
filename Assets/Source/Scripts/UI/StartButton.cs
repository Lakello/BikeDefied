using System;
using UnityEngine;

public class StartButton : MonoBehaviour, ISubscribe
{
    public event Action Action;

    public void OnClick()
    {
        Action?.Invoke();
    }
}
