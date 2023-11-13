using BikeDefied.Yandex.Saves.Data;
using System;

namespace BikeDefied.Yandex.Saves
{
    public class SaveAccessMethodsHolder<TData> 
        where TData : class, IPlayerData
    {
        public readonly Func<TData, TData> Getter;
        public readonly Action<TData> Setter;

        public SaveAccessMethodsHolder(Func<TData, TData> getter, Action<TData> setter)
        {
            Getter = getter;
            Setter = setter;
        }
    }
}