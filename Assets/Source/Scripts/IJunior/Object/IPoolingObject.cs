using System;
using UnityEngine;

public interface IPoolingObject<TInit>
{
    public Type SelfType { get; }
    public GameObject SelfGameObject { get; }

    public event System.Action<IPoolingObject<TInit>> Disable;

    public void Init(TInit init);
}