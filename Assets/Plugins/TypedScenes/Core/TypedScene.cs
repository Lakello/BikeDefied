using UnityEngine;
using UnityEngine.SceneManagement;

namespace IJunior.TypedScenes
{
    public abstract class TypedScene
    {
        protected static AsyncOperation LoadScene(string sceneName, LoadSceneMode loadSceneMode)
        {
            return SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        }

        protected static AsyncOperation LoadScene<T>(string sceneName, LoadSceneMode loadSceneMode, T argument)
        {
            LoadingProcessor.Instance.RegisterLoadingModel(argument);
            return SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        }

        protected static AsyncOperation LoadSceneInState<TState>(string sceneName, LoadSceneMode loadSceneMode)
        {

            return SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        }
    }
}
