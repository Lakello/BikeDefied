using Agava.YandexGames;
using Reflex.Core;
using UnityEngine;

public class InputHandlerInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private MobileControl _mobileControl;

    public void InstallBindings(ContainerDescriptor descriptor)
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
            inputHandler = new GameObject(nameof(PCInputHandler)).AddComponent<PCInputHandler>();
        }

        descriptor.AddInstance(inputHandler, typeof(IInputHandler));
    }
}