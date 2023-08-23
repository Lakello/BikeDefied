using Reflex.Attributes;
using System;

public class FlipCounter : ScoreCounter
{
    public override event Action<int> ScoreChanged;

    [Inject]
    protected override void Inject(Bike bike) => Init(bike);


}
