using System;
using System.Collections;
using Agava.YandexGames;
using UnityEngine;

namespace BikeDefied
{
    public class YandexInitializer : MonoBehaviour
    {
        private const int AuthorizationPollingDelay = 1500;

        private Action _callBack;

        private void Start() =>
            StartCoroutine(InitSDK());

        public void Init(Action sdkInitSuccessCallBack) =>
            _callBack = sdkInitSuccessCallBack;

        private IEnumerator InitSDK()
        {
#if !UNITY_EDITOR
            yield return YandexGamesSdk.Initialize();

            if (PlayerAccount.IsAuthorized == false)
                PlayerAccount.StartAuthorizationPolling(AuthorizationPollingDelay);
#endif
            _callBack();

            yield return null;
        }
    }
}