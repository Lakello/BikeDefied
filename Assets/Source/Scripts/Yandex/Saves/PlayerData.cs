using System;
using BikeDefied.Yandex.Saves.Data;

namespace BikeDefied.Yandex.Saves
{
    [Serializable]
    public class PlayerData
    {
        public LevelInfo[] LevelInfo = new LevelInfo[] { };
        public CurrentLevel CurrentLevel = new(4);
        public HintDisplay HintDisplay = new(true);
        public UnmuteSound UnmuteSound = new(1f);
    }
}
