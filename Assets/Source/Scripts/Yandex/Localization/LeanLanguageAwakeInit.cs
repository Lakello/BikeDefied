using Lean.Localization;
using UnityEngine;

namespace BikeDefied.Yandex.Localization
{
    [RequireComponent(typeof(LeanLocalization))]
    public class LeanLanguageAwakeInit : MonoBehaviour
    {
        private void Awake() =>
            GetComponent<LeanLocalization>().CurrentLanguage = GameLanguage.Value;
    }
}