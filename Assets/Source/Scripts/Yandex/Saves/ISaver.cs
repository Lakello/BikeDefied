using System;

public interface ISaver
{
    public TData Get<TData>(TData value = default) where TData : class, IPlayerData;

    public void Set<TData>(TData value = default) where TData : class, IPlayerData;

    public void SubscribeValueUpdated<TData>(Action<TData> subAction) where TData : class, IPlayerData;
    public void UnsubscribeValueUpdated<TData>(Action<TData> unsubAction) where TData : class, IPlayerData;
}