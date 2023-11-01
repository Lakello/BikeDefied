using BikeDefied.Other;
using BikeDefied.UI;
using BikeDefied.Yandex.Saves.Data;

namespace BikeDefied.LevelComponents
{
    public struct LevelViewInject
    {
        public SelectLevelScrollView SelectLevelScrollView { get; private set; }
        public IRead<CurrentLevel> CurrentLevelRead { get; private set; }
        public IWrite<CurrentLevel> CurrentLevelWrite { get; private set; }

        public LevelViewInject(System.Func<(SelectLevelScrollView, IRead<CurrentLevel>, IWrite<CurrentLevel>)> inject)
            => (SelectLevelScrollView, CurrentLevelRead, CurrentLevelWrite) = inject();
    }
}