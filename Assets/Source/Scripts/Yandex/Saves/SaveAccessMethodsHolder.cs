using System;

public delegate TData Getter<TData>(TData value = default) where TData : class, IPlayerData;
public delegate void Setter<TData>(TData value = default) where TData : class, IPlayerData;

public class SaveAccessMethodsHolder<TData> where TData : class, IPlayerData
{
    public readonly Getter<TData> Getter;
    public readonly Setter<TData> Setter;
    public event Action<TData> ValueUpdated
    {
        add => _valueUpdated += value;
        remove => _valueUpdated -= value;
    }

    protected Action<TData> _valueUpdated;

    public SaveAccessMethodsHolder(Getter<TData> getter, Setter<TData> setter) 
    {
        Getter = getter;
        Setter = setter;
    }
}