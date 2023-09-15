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

        var context = new GameObject("context").AddComponent<Context>();
        DontDestroyOnLoad(context);

        GameOverState over = new(context);
        var gameStatemachine = new GameStateMachine(() =>
        {
            return new System.Collections.Generic.Dictionary<System.Type, State<GameStateMachine>>()
            {
                [typeof(MenuState)] = new MenuState(),
                [typeof(PlayState)] = new PlayState(this, input),
                [typeof(GameOverState)] = over
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

        var saves = new Saves();

        var ad = new Ad(over, countOverBetweenShowsAd: 3, countOverBetweenShowsVideoAd: 10);

        var yandexInitializer = new GameObject("Init").AddComponent<YandexInitializer>();

        yandexInitializer.Init(sdkInitSuccessCallBack:() =>
        {
            saves.Init();
            ad.Show();
        });

        descriptor.AddInstance(saves, typeof(ISaver<CurrentLevel>), typeof(ISaverArray<LevelInfo>));
    }
}