using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using IJunior.StateMachine;

namespace IJunior.TypedScenes
{

    public class LoadingProcessor : MonoBehaviour
    {
        private static LoadingProcessor _instance;
        protected Action LoadingModelAction;

        public static LoadingProcessor Instance
        {
            get
            {
                if (_instance == null)
                    Initialize();

                return _instance;
            }
        }

        private static void Initialize()
        {
            _instance = new GameObject("LoadingProcessor").AddComponent<LoadingProcessor>();
            _instance.transform.SetParent(null);
            DontDestroyOnLoad(_instance);
        }

        public void ApplyLoadingModel()
        {
            LoadingModelAction?.Invoke();
            LoadingModelAction = null;
        }

        public void RegisterLoadingModel<TMachine, TState>() where TMachine : StateMachine<TMachine> where TState : State<TMachine>
        {
            LoadingModelAction = () =>
            {
                foreach (var rootObjects in SceneManager.GetActiveScene().GetRootGameObjects())
                {
                    foreach (var handler in rootObjects.GetComponentsInChildren<ISceneLoadHandler<TMachine>>())
                    {
                        handler.OnSceneLoaded<TState>();
                    }
                }
            };
        }
    }
}
