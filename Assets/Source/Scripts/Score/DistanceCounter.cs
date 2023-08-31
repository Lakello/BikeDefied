using System;

public class DistanceCounter : ScoreCounter
{
    private float _startPosition;
    private float _bestPosition;
    private float _reward;

    public override string Name => "Distance";

    private float CurrentDistance => _bestPosition - _startPosition;
    private float CurrentPosition => BikeBody.position.z;

    public DistanceCounter(float reward, ScoreCounterInject inject) : base(inject) { _reward = reward; }

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
            Reward.Value = _reward;

            ScoreAdd?.Invoke(Reward);
        }
    }
}