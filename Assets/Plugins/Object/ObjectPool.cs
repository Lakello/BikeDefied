using System.Collections.Generic;

namespace IJunior.Object
{
    public class ObjectPool<TInit>
    {
        private Dictionary<System.Type, Queue<IPoolingObject<TInit>>> _objects = new Dictionary<System.Type, Queue<IPoolingObject<TInit>>>();

        public void Return(IPoolingObject<TInit> @object)
        {
            UnityEngine.Debug.Log("ENTER RETURN");
            @object.Disable -= Return;

            Add(@object);
        }

        public IPoolingObject<TInit> TryGetObjectByType(System.Type objectType)
        {
            UnityEngine.Debug.Log("ENTER TRY GET OBJECT BY TYPE");
            if (_objects.TryGetValue(objectType, out Queue<IPoolingObject<TInit>> playersData))
            {
                if (playersData.Count > 0)
                {
                    var data = playersData.Dequeue();

                    return data;
                }
            }

            return null;
        }

        private void Add(IPoolingObject<TInit> @object)
        {
            UnityEngine.Debug.Log("ENTER ADD");
            if (!_objects.ContainsKey(@object.SelfType))
                AddType(@object.SelfType);

            if (_objects.TryGetValue(@object.SelfType, out Queue<IPoolingObject<TInit>> playersData))
            {
                @object.SelfGameObject.SetActive(false);
                playersData.Enqueue(@object);
            }
        }

        private void AddType(System.Type type)
        {
            UnityEngine.Debug.Log("ENTER ADD TYPE");
            _objects.Add(type, new Queue<IPoolingObject<TInit>>());
        }
    }
}
