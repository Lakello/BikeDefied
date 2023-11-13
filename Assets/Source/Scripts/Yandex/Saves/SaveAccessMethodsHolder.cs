﻿using BikeDefied.Yandex.Saves.Data;
using System;

namespace BikeDefied.Yandex.Saves
{
    public class SaveAccessMethodsHolder<TData> 
        where TData : class, IPlayerData
    {
        public Func<TData, TData> Getter { get; }
        public Action<TData> Setter { get; }

        public SaveAccessMethodsHolder(Func<TData, TData> getter, Action<TData> setter)
        {
            Getter = getter;
            Setter = setter;
        }
    }
}