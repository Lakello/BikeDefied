﻿using Agava.YandexGames;
using IJunior.StateMachine;
using Lean.Localization;
using Reflex.Core;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class ProjectInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private GameAudioHandler _gameAudioHandler;
    [SerializeField] private TutorialCanvas _tutorialPrefab;

    public void InstallBindings(ContainerDescriptor descriptor)
    {
        var input = new PlayerInput();

        descriptor.AddInstance(input);

        var context = new GameObject(nameof(Context)).AddComponent<Context>();
        DontDestroyOnLoad(context);

        var backgoundAudio = new GameObject(nameof(AudioSource)).AddComponent<AudioSource>();
        var gameAudio = backgoundAudio.AddComponent<AudioSource>();
        DontDestroyOnLoad(backgoundAudio);
        _gameAudioHandler.Init();
        var audioController = new AudioController(gameAudio, backgoundAudio, _gameAudioHandler, context);

        descriptor.AddInstance(audioController, typeof(IAudioController));

        var playState = new PlayState(this, input, audioController);
        var overState = new OverState(context);
        var gameStatemachine = new GameStateMachine(() =>
        {
            return new Dictionary<System.Type, State<GameStateMachine>>()
            {
                [typeof(MenuState)] = new MenuState(),
                [typeof(PlayState)] = playState,
                [typeof(OverState)] = overState
            };
        });

        var windowStateMachine = new WindowStateMachine(() =>
        {
            return new Dictionary<System.Type, State<WindowStateMachine>>()
            {
                [typeof(MenuWindowState)] = new MenuWindowState(),
                [typeof(PlayWindowState)] = new PlayWindowState(),
                [typeof(OverWindowState)] = new OverWindowState(),
                [typeof(LeaderboardWindowState)] = new LeaderboardWindowState()
            };
        });

        var saves = new GamePlayerDataSaver();

        var ad = new Ad(overState, countOverBetweenShowsAd: 3, countOverBetweenShowsVideoAd: 10);

        var yandexInitializer = new GameObject("Init").AddComponent<YandexInitializer>();

        yandexInitializer.Init(sdkInitSuccessCallBack:() =>
        {
            saves.Init();
            ad.Show();

            string lang = "ru";
#if !UNITY_EDITOR
            lang = YandexGamesSdk.Environment.i18n.lang;
#endif
            GameLanguage.Value = lang;

            if (saves.Get<NotFirstSession>().IsNotFirstSession == false)
            {
                var tutorial = Instantiate(_tutorialPrefab);
                DontDestroyOnLoad(tutorial);
                saves.Set(new NotFirstSession(true));
            }

            return true;
        });

        descriptor.AddInstance(saves, typeof(ISaver));
    }
}