using UnityEngine;
using UnityEngine.UI;

public class SelectLevelButtonsView : MonoBehaviour
{
    private ScrollRect _view;

    private void Awake()
    {
        _view = GetComponent<ScrollRect>();

        _view.FocusOnItem(_view.content.GetChild(9).GetComponent<RectTransform>());
    }
}