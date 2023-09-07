public struct BikeBehaviourInject
{
    public Player Player { get; private set; }
    public Bike BikeBody { get; private set; }

    
    public BikeBehaviourInject(System.Func<(Player, Bike)> inject) => (Player, BikeBody) = inject();
}