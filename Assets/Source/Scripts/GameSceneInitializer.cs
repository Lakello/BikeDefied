using BikeDefied.FSM.Game;
using BikeDefied.FSM.GameWindow;
using BikeDefied.FSM;
using BikeDefied.TypedScenes;
using System.Collections.Generic;
using UnityEngine;

namespace BikeDefied
{
    public class GameSceneInitializer : MonoBehaviour, ISceneLoadHandlerOnState<GameStateMachine>
    {
        [SerializeField] private List<Window> _windows;

        public void OnSceneLoaded<TState>(GameStateMachine machine) where TState : State<GameStateMachine>
        {
            WindowsInit(machine);

            machine.EnterIn<TState>();
        }

        private void WindowsInit(GameStateMachine machine)
        {
            foreach (var window in _windows)
            {
                var state = machine.WindowStateMachine.TryGetState<WindowState>(window);
                
                state.Init(window);
            }
        }
    }
}