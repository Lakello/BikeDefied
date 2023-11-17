using UnityEngine;

namespace BikeDefied.Tutorial
{
    public class Hint : MonoBehaviour
    {
        [SerializeField] private bool _forDesktop;
        [SerializeField] private bool _forMobile;
        [SerializeField] private bool _alwaysShowMobile;
        [SerializeField] private bool _alwaysShowDesktop;

        private bool IsMobile => Application.isMobilePlatform;

        private bool CanShow => (IsMobile == true && _forMobile == true) || (IsMobile == false && _forDesktop == true);

        public void StartShow(bool value)
        {
            if (!TryShow(value) && CanShow)
            {
                gameObject.SetActive(value);
            }
        }

        public void HindDisplayUpdated(bool value)
        {
            if (!TryShow(value) && CanShow)
            {
                if (value)
                {
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }

        private bool TryShow(bool value)
        {
            if ((IsMobile == true && _alwaysShowMobile == true && value == false)
                || (IsMobile == false && _alwaysShowDesktop == true && value == false))
            {
                gameObject.SetActive(true);
                return true;
            }

            return false;
        }
    }
}