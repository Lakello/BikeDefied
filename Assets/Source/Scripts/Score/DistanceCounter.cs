using System;

public class DistanceCounter : ScoreCounter
{
    private float _startPosition;
    private float _bestPosition;

    private float CurrentDistance => _bestPosition - _startPosition;
    private float CurrentPosition => BikeBody.position.z;

    public DistanceCounter(ScoreCounterInject inject) : base(inject) { }

    public override event Action<IReward> ScoreAdd;

    protected override void Start()
    {
        _startPosition = _bestPosition = CurrentPosition;

        BehaviourCoroutine = Context.StartCoroutine(Player.Behaviour(
        condition: () =>
        {
            return IsGrounded;
        },
        action: () =>
        {
            TryAddScore();
        }));
    }

    private void TryAddScore()
    {
        if (CurrentPosition > _bestPosition)
        {
            _bestPosition = CurrentPosition;

            Reward.Message = "";
            Reward.Value = 1;

            ScoreAdd?.Invoke(Reward);
        }
    }
}