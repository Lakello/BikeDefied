using BikeDefied.BikeSystem;
using BikeDefied.Game;
using UnityEngine;

namespace BikeDefied.ScoreSystem
{
    public struct ScoreCounterInject
    {
        public ScoreCounterInject(System.Func<(Player, MonoBehaviour, GroundChecker, Bike)> inject) =>
            (Player, Context, GroundChecker, BikeBody) = inject();
        
        public Player Player { get; private set; }
        
        public MonoBehaviour Context { get; private set; }
        
        public GroundChecker GroundChecker { get; private set; }
        
        public Bike BikeBody { get; private set; }
    }
}