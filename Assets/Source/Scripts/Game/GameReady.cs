using Agava.YandexGames;
using IJunior.StateMachine;
using IJunior.TypedScenes;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameReady : MonoBehaviour, ISceneLoadHandlerState<GameStateMachine>
{
    public void OnSceneLoaded<TState>() where TState : State<GameStateMachine>
    {
        YandexGamesSdk.SetReady();
    }
}