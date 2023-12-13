using System;
using System.Collections.Generic;
using Agava.YandexGames;
using BikeDefied.AudioSystem;
using BikeDefied.FSM;
using BikeDefied.FSM.Game;
using BikeDefied.FSM.Game.States;
using BikeDefied.FSM.GameWindow;
using BikeDefied.FSM.GameWindow.States;
using BikeDefied.Other;
using BikeDefied.TypedScenes;
using BikeDefied.Yandex;
using BikeDefied.Yandex.AD;
using BikeDefied.Yandex.Localization;
using BikeDefied.Yandex.Saves;
using Reflex.Core;
using UnityEngine;

namespace BikeDefied
{
    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private GameAudioHandler _gameAudioHandler;

        public void InstallBindings(ContainerDescriptor descriptor)
        {
            var input = InputInit(descriptor);

            var context = new GameObject(nameof(Context)).AddComponent<Context>();
            DontDestroyOnLoad(context);

            var focusObserver = new GameObject(nameof(FocusObserver)).AddComponent<FocusObserver>();
            DontDestroyOnLoad(focusObserver);

            AudioInit(descriptor, focusObserver, context);

            var gameStateMachine = StateMachineInit(descriptor, input, context);

            YandexInit(descriptor, focusObserver, gameStateMachine);
        }

        private PlayerInput InputInit(ContainerDescriptor descriptor)
        {
            var input = new PlayerInput();

            descriptor.AddInstance(input);

            return input;
        }

        private void AudioInit(ContainerDescriptor descriptor, FocusObserver focusObserver, Context context)
        {
            var backgoundAudio = new GameObject(nameof(AudioSource)).AddComponent<AudioSource>();
            var gameAudio = backgoundAudio.gameObject.AddComponent<AudioSource>();
            DontDestroyOnLoad(backgoundAudio);

            _gameAudioHandler.Init();
            var audioController = new AudioVolumeChanger(gameAudio, backgoundAudio, _gameAudioHandler, context);

            _ = new AudioFocusObserver(gameAudio, backgoundAudio, focusObserver);

            descriptor.AddInstance(audioController, typeof(IAudioVolumeChanger));
        }

        private GameStateMachine StateMachineInit(ContainerDescriptor descriptor, PlayerInput input, Context context)
        {
            var windowStateMachine = new WindowStateMachine(() =>
            {
                return new Dictionary<Type, State<WindowStateMachine>>()
                {
                    [typeof(MenuWindowState)] = new MenuWindowState(),
                    [typeof(PlayWindowState)] = new PlayWindowState(),
                    [typeof(OverWindowState)] = new OverWindowState(),
                    [typeof(LeaderboardWindowState)] = new LeaderboardWindowState(),
                };
            });

            var gameOverState = new EndLevelState(context, windowStateMachine);
            var gamePlayState = new PlayLevelState(input, windowStateMachine);
            var gameMenuState = new MenuState(windowStateMachine);

            var stateInject = new GameStateInject(() => (gameMenuState, gamePlayState, gameOverState));

            descriptor.AddInstance(stateInject);

            return new GameStateMachine(windowStateMachine, () =>
            {
                return new Dictionary<Type, State<GameStateMachine>>()
                {
                    [typeof(MenuState)] = gameMenuState,
                    [typeof(PlayLevelState)] = gamePlayState,
                    [typeof(EndLevelState)] = gameOverState,
                };
            });
        }

        private void YandexInit(ContainerDescriptor descriptor, FocusObserver focusObserver, GameStateMachine gameStateMachine)
        {
            var saves = new GamePlayerDataSaver();

            var ad = new Ad(focusObserver, countOverBetweenShowsAd: 5);
            descriptor.AddInstance(ad, typeof(ICounterForShowAd));

            var yandexInitializer = new GameObject(nameof(YandexInitializer)).AddComponent<YandexInitializer>();

            yandexInitializer.Init(sdkInitSuccessCallBack: () =>
            {
                saves.Init();

                string lang = "ru";
#if !UNITY_EDITOR
                lang = YandexGamesSdk.Environment.i18n.lang;
#endif
                GameLanguage.Value = lang;

                GameScene.Load<MenuState>(gameStateMachine);
            });

            descriptor.AddInstance(saves, typeof(ISaver));
        }
    }
}