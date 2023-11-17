using System;
using UnityEngine;

namespace BikeDefied.Game.Spawner
{
    public interface IPoolingObject<TInit>
    {
        public event Action<IPoolingObject<TInit>> Disabled;
        
        public Type SelfType { get; }
        
        public GameObject SelfGameObject { get; }

        public void Init(TInit init);
    }
}