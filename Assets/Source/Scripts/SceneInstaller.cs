using Reflex.Core;
using UnityEngine;

public class SceneInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private CharacterHead _characterHead;

    private Game _game = new();
 
    public void InstallBindings(ContainerDescriptor descriptor)
    {
        var input = new PlayerInput();
        input.Enable();
        var inputHandler = new PCInputHandler(input);

        descriptor.AddInstance(input);
        descriptor.AddInstance(inputHandler, typeof(IInputHandler));
        descriptor.AddInstance(_game, typeof(IGameOver));
    }

    private void OnEnable()
    {
        _characterHead.Crash += _game.OnCrash;
    }

    private void OnDisable()
    {
        _characterHead.Crash -= _game.OnCrash;
    }
}
