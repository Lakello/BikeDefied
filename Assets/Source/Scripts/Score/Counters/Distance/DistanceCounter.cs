using System;

namespace BikeDefied.ScoreSystem
{
    public class DistanceCounter : ScoreCounter
    {
        private float _bestPosition;
        private float _reward;

        private float CurrentPosition => BikeBody.position.z;

        public DistanceCounter(float reward, ScoreCounterInject inject) : base(inject) =>
            _reward = reward;

        public override event Action<ScoreReward> ScoreAdding;

        protected override void Start()
        {
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

                Reward.Message = string.Empty;
                Reward.Value = _reward;

                ScoreAdding?.Invoke(Reward);
            }
        }
    }
}