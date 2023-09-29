using UnityEngine;
using UnityEngine.UI;

public class UnmuteControlToggleView : MonoBehaviour
{
    [SerializeField] private Sprite _disable;
    [SerializeField] private Sprite _enable;
    [SerializeField] private Image _image;
    
    public void OnChangeImage(bool value) =>
        _image.sprite = value ? _enable : _disable;
}