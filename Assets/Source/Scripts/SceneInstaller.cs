using Reflex.Core;
using UnityEngine;

public class SceneInstaller : MonoBehaviour, IInstaller
{
    [SerializeField, Range(0, 1)] private float _weightForPhysicsFollow = 1.0f;

    public void InstallBindings(ContainerDescriptor descriptor)
    {
        var input = new PlayerInput();
        input.Enable();
        var inputHandler = new PCInputHandler(input);

        descriptor.AddInstance(_weightForPhysicsFollow, typeof(float));
        descriptor.AddInstance(input);
        descriptor.AddInstance(inputHandler, typeof(IInputHandler));
    }
}
