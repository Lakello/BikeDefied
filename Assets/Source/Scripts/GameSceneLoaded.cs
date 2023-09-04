﻿using IJunior.StateMachine;
using IJunior.TypedScenes;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneLoaded : MonoBehaviour, ISceneLoadHandlerState<GameStateMachine>
{
    [SerializeField] private List<Window> _windows;

    public void OnSceneLoaded<TState>() where TState : State<GameStateMachine>
    {
        WindowsInit();

        GameStateMachine.Instance.EnterIn<TState>();
    }

    private void WindowsInit()
    {
        foreach (var window in _windows)
        {
            var state = WindowStateMachine.Instance.GetState<WindowState>(window);

            Debug.Log($"State {state}");

            state.Init(window);
        }
    }
}