using Reflex.Core;
using UnityEngine;

public class SceneInstaller : MonoBehaviour, IInstaller
{
    public void InstallBindings(ContainerDescriptor descriptor)
    {
        var input = new PlayerInput();
        input.Enable();
        var inputHandler = new PCInputHandler(input);

        descriptor.AddInstance(input);
        descriptor.AddInstance(inputHandler, typeof(IInputHandler));
    }
}
