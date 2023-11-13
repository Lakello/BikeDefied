using BikeDefied.Yandex.Saves.Data;
using System;

namespace BikeDefied.Yandex.Saves
{
    [Serializable]
    public class PlayerData
    {
        public LevelInfo[] LevelInfo = new LevelInfo[] { };
        public CurrentLevel CurrentLevel = new CurrentLevel(4);
        public HintDisplay HintDisplay = new HintDisplay(true);
        public UnmuteSound UnmuteSound = new UnmuteSound(1f);
    }
}
