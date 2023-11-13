using System.Linq;
using UnityEngine;

namespace BikeDefied.TypedScenes
{
    public class TypedProcessor : MonoBehaviour
    {
        private void Awake()
        {
            foreach (ITypedAwakeHandler handler in FindObjectsOfType<MonoBehaviour>().OfType<ITypedAwakeHandler>())
            {
                handler.OnSceneAwake();
            }

            LoadingProcessor.Instance.ApplyLoadingModel();
        }
    }
}
