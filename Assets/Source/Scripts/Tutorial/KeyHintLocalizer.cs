using BikeDefied.Yandex.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace BikeDefied.Tutorial
{
    public class KeyHintLocalizer : MonoBehaviour
    {
        [SerializeField] private Sprite _ru;
        [SerializeField] private Sprite _en;
        [SerializeField] private Sprite _tr;

        [SerializeField] private Image _image;

        private void Start()
        {
            _image.sprite = GameLanguage.Value switch
            {
                "ru" => _ru,
                "en" => _en,
                "tr" => _tr,
                _ => _ru,
            };
        }
    }
}