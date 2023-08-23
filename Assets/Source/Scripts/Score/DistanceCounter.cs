using Reflex.Attributes;
using Reflex.Core;
using System;

public class DistanceCounter : ScoreCounter, IStartable
{
    private Bike _bike;

    private int _startPosition;
    private int _bestPosition;
    private int _currentPosition;

    public override event Action<int> ScoreChanged;

    public void Start()
    {
        _startPosition = _bestPosition = _currentPosition
            = (int)_bike.transform.position.z;
    }

    [Inject]
    protected override void Inject(Bike bike) => Init(bike);


}
