using Reflex.Core;
using System.Collections.Generic;
using UnityEngine;

public class SceneInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private CharacterHead _characterHead;
    [SerializeField] private Bike _bike;

    private Game _game = new();
    private List<IScoreCounter> _scoreCounters;
 
    public void InstallBindings(ContainerDescriptor descriptor)
    {
        var input = new PlayerInput();
        input.Enable();
        var inputHandler = new PCInputHandler(input);

        descriptor.AddInstance(input);
        descriptor.AddInstance(inputHandler, typeof(IInputHandler));

        descriptor.AddInstance(_game, typeof(IGameOver));

        descriptor.AddInstance(_bike);
        _scoreCounters = new List<IScoreCounter>()
        {
            new DistanceCounter(),
            new FlipCounter()
        };
        descriptor.AddInstance(_scoreCounters, typeof(IReadOnlyList<IScoreCounter>));
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
