using BikeDefied.Yandex.AD;
using Reflex.Attributes;

namespace BikeDefied.UI.Buttons
{
    public class CounterForShowAdButton : EventTriggerButton
    {
        private ICounterForShowAd _counter;

        [Inject]
        private void Inject(ICounterForShowAd counter) =>
            _counter = counter;

        public override void OnClick()
        {
            base.OnClick();
            _counter.Add();
        }
    }
}