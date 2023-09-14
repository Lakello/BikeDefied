using UnityEngine;

namespace IJunior.Object
{
    public class ObjectFactory<TInit>
    {
        private readonly Transform _parent;

        public ObjectFactory(Transform parent) =>
            _parent = parent;

        public IPoolingObject<TInit> GetNewObject(IPoolingObject<TInit> prefab)
        {
            var newObject = UnityEngine.Object.Instantiate(prefab.SelfGameObject);
            newObject.transform.SetParent(_parent);
            newObject.SetActive(false);
            return newObject.GetComponent<IPoolingObject<TInit>>();
        }
    }
}
