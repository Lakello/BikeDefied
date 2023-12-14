using System;
using BikeDefied.Yandex.Saves.Data;

namespace BikeDefied.Yandex.Saves
{
    public class SaveAccessMethodsHolder<TData>
        where TData : class, IPlayerData
    {
        public SaveAccessMethodsHolder(Func<TData, TData> getter, Action<TData> setter)
        {
            Getter = getter;
            Setter = setter;
        }

        public Func<TData, TData> Getter { get; }

        public Action<TData> Setter { get; }
    }
}