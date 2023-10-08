using Agava.YandexGames;
using System;
using System.Collections;
using UnityEngine;

public class YandexInitializer : MonoBehaviour
{
    public void Init(Action sdkInitSuccessCallBack) =>
        StartCoroutine(InitSDK(sdkInitSuccessCallBack));

    private IEnumerator InitSDK(Action sdkInitSuccessCallBack)
    {
#if !UNITY_EDITOR
        yield return YandexGamesSdk.Initialize(sdkInitSuccessCallBack);

        if (PlayerAccount.IsAuthorized == false)
            PlayerAccount.StartAuthorizationPolling(1500);
#endif

        IJunior.TypedScenes.Game.Load<MenuState>();

        yield return null;
    }
}
