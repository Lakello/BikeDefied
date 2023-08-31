using IJunior.StateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IJunior.TypedScenes
{
    public abstract class TypedScene<TMachine> where TMachine : StateMachine<TMachine>
    {
        protected static AsyncOperation LoadScene(string sceneName, LoadSceneMode loadSceneMode)
        {
            return SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        }

        protected static AsyncOperation LoadScene<TState>(string sceneName, LoadSceneMode loadSceneMode) where TState : State<TMachine>
        {
            LoadingProcessor.Instance.RegisterLoadingModel<TMachine, TState>();
            return SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        }

        protected static AsyncOperation LoadScene<T>(string sceneName, LoadSceneMode loadSceneMode, T argument)
        {
            LoadingProcessor.Instance.RegisterLoadingModel(argument);
            return SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        }
    }
}
