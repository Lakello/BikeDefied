using Agava.YandexGames;
using BikeDefied.TypedScenes;
using UnityEngine;

namespace BikeDefied.Yandex
{
    public class GameReady : MonoBehaviour, ISceneLoadHandler
    {
        public void OnSceneLoaded()
        {
#if !UNITY_EDITOR
            YandexGamesSdk.Ready();
#endif
        }
    }
}