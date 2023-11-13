using BikeDefied.Yandex.Saves.Data;
using System;
using UnityEngine;

namespace BikeDefied.Yandex.Saves
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private LevelInfo[] _levelInfo = new LevelInfo[] { };
        [SerializeField] private CurrentLevel _currentLevel = new CurrentLevel(4);
        [SerializeField] private HintDisplay _hintDisplay = new HintDisplay(true);
        [SerializeField] private UnmuteSound _unmuteSound = new UnmuteSound(1f);

        public LevelInfo[] LevelInfo { get => _levelInfo; set => _levelInfo = value; }
        public CurrentLevel CurrentLevel { get => _currentLevel; set => _currentLevel = value; }
        public HintDisplay HintDisplay { get => _hintDisplay; set => _hintDisplay = value; }
        public UnmuteSound UnmuteSound { get => _unmuteSound; set => _unmuteSound = value; }
    }
}
