using Agava.YandexGames;
using System;
using System.Collections;
using UnityEngine;

public class YandexInitializer : MonoBehaviour
{
    private Action _callBack;

    private void Start()
    {
        StartCoroutine(InitSDK());
    }

    public void Init(Action sdkInitSuccessCallBack) =>
        _callBack = sdkInitSuccessCallBack;

    private IEnumerator InitSDK()
    {
#if !UNITY_EDITOR
        yield return YandexGamesSdk.Initialize(_callBack);

        if (PlayerAccount.IsAuthorized == false)
            PlayerAccount.StartAuthorizationPolling(1500);
#else
        _callBack();
#endif
        IJunior.TypedScenes.Game.Load<MenuState>();

        yield return null;
    }
}
