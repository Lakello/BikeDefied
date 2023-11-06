using System;
using UnityEngine;

namespace BikeDefied.Game.Spawner
{
    public interface IPoolingObject<TInit>
    {
        public Type SelfType { get; }
        public GameObject SelfGameObject { get; }

        public event System.Action<IPoolingObject<TInit>> Disabled;

        public void Init(TInit init);
    }
}