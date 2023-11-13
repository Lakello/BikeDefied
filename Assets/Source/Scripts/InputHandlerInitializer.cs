using BikeDefied.InputSystem;
using Reflex.Core;
using UnityEngine;

namespace BikeDefied
{
    public class InputHandlerInitializer : MonoBehaviour
    {
        [SerializeField] private MobileControl _mobileControl;

        public IInputHandler Init()
        {
            IInputHandler inputHandler;

            if (Application.isMobilePlatform)
            {
                var mobileInputHandler = new GameObject(nameof(MobileInputHandler)).AddComponent<MobileInputHandler>();
                _mobileControl.gameObject.SetActive(true);
                mobileInputHandler.Init(_mobileControl);

                inputHandler = mobileInputHandler;
            }
            else
            {
                inputHandler = new GameObject(nameof(DesktopInputHandler)).AddComponent<DesktopInputHandler>();
            }

            return inputHandler;
        }
    }
}