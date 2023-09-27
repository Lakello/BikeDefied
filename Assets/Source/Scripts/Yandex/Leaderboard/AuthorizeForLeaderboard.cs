using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;

public class AuthorizeForLeaderboard : MonoBehaviour
{
    [SerializeField] private EventTriggerButton[] _disablingButtons;
    [SerializeField] private bool _isAuthorizedSim;

    private void OnEnable()
    {
        bool isAuthorized = _isAuthorizedSim;
#if !UNITY_EDITOR
        isAuthorized = PlayerAccount.IsAuthorized;
#endif

        if (isAuthorized)
        {
            gameObject.SetActive(false);
            return;
        }

        foreach (var button in _disablingButtons)
            button.IsInteractable = false;
    }

    private void OnDisable()
    {
        foreach (var button in _disablingButtons)
            button.IsInteractable = true;
    }

    public void OnAuthorizeClick() =>
        PlayerAccount.Authorize();

    public void OnCancelClick() =>
        gameObject.SetActive(false);
}
