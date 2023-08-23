using System;

public class FlipCounter : ScoreCounter
{
    public override event Action<float> ScoreUpdated;

    public FlipCounter(ScoreCounterInject inject) : base(inject) { }

    protected override void Start()
    {
        
    }
}
