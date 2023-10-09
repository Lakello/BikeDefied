using Agava.YandexGames;
using System;
using System.Collections;
using UnityEngine;

public class YandexInitializer : MonoBehaviour
{
    private Func<bool> _sdkInitSuccessCallBack;

    private void Start()
    {
        StartCoroutine(InitSDK());
    }

    public void Init(Func<bool> sdkInitSuccessCallBack)
    {
        _sdkInitSuccessCallBack = sdkInitSuccessCallBack;
    }

    private IEnumerator InitSDK()
    {
#if !UNITY_EDITOR
        yield return YandexGamesSdk.Initialize();

        if (PlayerAccount.IsAuthorized == false)
            PlayerAccount.StartAuthorizationPolling(1500);
#endif

        yield return new WaitUntil(_sdkInitSuccessCallBack);

        IJunior.TypedScenes.Game.Load<MenuState>();
    }
}
