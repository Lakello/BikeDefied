using UnityEngine;

public class TutorialControlView : MonoBehaviour
{
    [SerializeField] private GameObject _desktopInfo;
    [SerializeField] private GameObject _mobileInfo;

    private void OnEnable()
    {
        if (Application.isMobilePlatform)
        {
            _desktopInfo.SetActive(false);
            _mobileInfo.SetActive(true);
        }
        else
        {
            _desktopInfo.SetActive(true);
            _mobileInfo.SetActive(false);
        }
    }
}