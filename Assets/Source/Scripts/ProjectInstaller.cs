using IJunior.StateMachine;
using Reflex.Core;
using UnityEngine;

public class ProjectInstaller : MonoBehaviour, IInstaller
{
    public void InstallBindings(ContainerDescriptor descriptor)
    {
        var input = new PlayerInput();
        var inputHandler = new PCInputHandler(input);

        descriptor.AddInstance(inputHandler, typeof(IInputHandler));

        var gameStatemachine = new GameStateMachine(() =>
        {
            return new System.Collections.Generic.Dictionary<System.Type, State<GameStateMachine>>()
            {
                [typeof(MenuState)] = new MenuState(),
                [typeof(PlayState)] = new PlayState(this, input),
                [typeof(GameOverState)] = new GameOverState()
            };
        });

        var windowStateMachine = new WindowStateMachine(() =>
        {
            return new System.Collections.Generic.Dictionary<System.Type, State<WindowStateMachine>>()
            {
                [typeof(MenuWindowState)] = new MenuWindowState(),
                [typeof(PlayWindowState)] = new PlayWindowState(),
                [typeof(GameOverWindowState)] = new GameOverWindowState()
            };
        });
    }
}