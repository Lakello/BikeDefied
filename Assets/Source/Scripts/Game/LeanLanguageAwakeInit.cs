using Lean.Localization;
using UnityEngine;

[RequireComponent(typeof(LeanLocalization))]
public class LeanLanguageAwakeInit : MonoBehaviour
{
    private void Awake() =>
        GetComponent<LeanLocalization>().CurrentLanguage = GameLanguage.Value;
}
