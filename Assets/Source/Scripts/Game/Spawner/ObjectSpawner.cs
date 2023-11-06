using UnityEngine;

namespace BikeDefied.Game.Spawner
{
    public class ObjectSpawner<TInit>
    {
        private ObjectFactory<TInit> _factory;
        private ObjectPool<TInit> _pool;

        public ObjectSpawner(ObjectFactory<TInit> factory, ObjectPool<TInit> pool)
        {
            _factory = factory;
            _pool = pool;
        }

        public IPoolingObject<TInit> Spawn(IPoolingObject<TInit> @object, TInit init = default, System.Func<Vector3> getSpawnPosition = null)
        {
            IPoolingObject<TInit> spawningObject = GetObject(@object);

            if (getSpawnPosition != null)
            {
                Vector3 position = getSpawnPosition();
                spawningObject.SelfGameObject.transform.position = position;
            }

            spawningObject.Disabled += _pool.Return;

            if (init != null)
                spawningObject.Init(init);

            spawningObject.SelfGameObject.SetActive(true);

            return spawningObject;
        }
    

        private IPoolingObject<TInit> GetObject(IPoolingObject<TInit> @object)
        {
            IPoolingObject<TInit> spawningObject = _pool.TryGetObjectByType(@object.SelfType) ?? CreateObject(@object);

            return spawningObject;
        }

        private IPoolingObject<TInit> CreateObject(IPoolingObject<TInit> @object)
        {
            IPoolingObject<TInit> newObject = _factory.GetNewObject(@object);

            return newObject;
        }
    }
}
