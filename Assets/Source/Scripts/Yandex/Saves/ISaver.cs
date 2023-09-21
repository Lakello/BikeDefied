using System;

public interface ISaver
{
    public TData Get<TData>(TData value = default) where TData : class, IPlayerData;

    public void Set<TData>(TData value = default) where TData : class, IPlayerData;

    public Action<TData> ValueUpdated<TData>() where TData : class, IPlayerData;
}