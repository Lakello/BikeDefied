using Agava.YandexGames;
using IJunior.TypedScenes;
using UnityEngine;

public class GameReady : MonoBehaviour, ISceneLoadHandler
{
    public void OnSceneLoaded()
    {
#if !UNITY_EDITOR
        YandexGamesSdk.Ready();
#endif
    }
}