using System;

public class DistanceCounter : ScoreCounter
{
    private float _startPosition;
    private float _bestPosition;
    private float _reward;

    public override string Name => "Distance";

    private float CurrentPosition => BikeBody.position.z;

    public DistanceCounter(float reward, ScoreCounterInject inject) : base(inject) { _reward = reward; }

    public override event Action<IReward> ScoreAdd;

    protected override void Start()
    {
        _startPosition = _bestPosition = CurrentPosition;

        BehaviourCoroutine = Context.StartCoroutine(Player.Behaviour(
        condition: () =>
        {
            return true;
        },
        action: () =>
        {
            TryAddScore();
        }));
    }

    private void TryAddScore()
    {
        if (MathF.Round(CurrentPosition, 1) > MathF.Round(_bestPosition, 1))
        {
            _bestPosition = CurrentPosition;

            Reward.Message = "";
            Reward.Value = _reward;

            ScoreAdd?.Invoke(Reward);
        }
    }
}