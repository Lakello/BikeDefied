using System;

namespace BikeDefied.ScoreSystem
{
    public class DistanceCounter : ScoreCounter
    {
        private float _bestPosition;
        private float _reward;

        public DistanceCounter(float reward, ScoreCounterInject inject)
            : base(inject) =>
            _reward = reward;

        public override event Action<ScoreReward> ScoreAdding;

        private float CurrentPosition => BikeBody.position.z;

        protected override void Start()
        {
            BehaviourCoroutine = Context.StartCoroutine(Player.Behaviour(
                condition: () => true,
                action: TryAddScore));
        }

        private void TryAddScore()
        {
            if (!(MathF.Round(CurrentPosition, 1) > MathF.Round(_bestPosition, 1)))
            {
                return;
            }

            _bestPosition = CurrentPosition;

            ScoreReward reward = new ScoreReward
            {
                Message = String.Empty, Value = _reward,
            };

            ScoreAdding?.Invoke(reward);
        }
    }
}