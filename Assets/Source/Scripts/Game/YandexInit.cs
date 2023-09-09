using Agava.YandexGames;
using System;
using System.Collections;
using UnityEngine;

public class YandexInit : MonoBehaviour
{
    private Action _initSuccessCallBack;

    private void Start()
    {
        StartCoroutine(InitSDK());
    }

    public void Init(Action initSuccessCallBack)
    {
        _initSuccessCallBack = initSuccessCallBack;
    }

    private IEnumerator InitSDK()
    {
#if !UNITY_EDITOR
        // Always wait for it if invoking something immediately in the first scene.
        yield return YandexGamesSdk.Initialize();

        if (PlayerAccount.IsAuthorized == false)
            PlayerAccount.StartAuthorizationPolling(1500);
#endif
        
        _initSuccessCallBack?.Invoke();

        yield return null;

        IJunior.TypedScenes.Game.Load<MenuState>();
    }
}
