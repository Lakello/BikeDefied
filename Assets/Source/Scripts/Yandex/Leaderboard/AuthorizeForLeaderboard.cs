using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;

public class AuthorizeForLeaderboard : MonoBehaviour
{
    [SerializeField] private EventTriggerButton[] _disablingButtons;

    private void OnEnable()
    {
        if (PlayerAccount.IsAuthorized)
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
