using BikeDefied.Yandex.AD;
using Reflex.Attributes;

namespace BikeDefied.UI.Buttons
{
    public class RestartButton : EventTriggerButton
    {
        private ICounterForShowAd _counter;

        [Inject]
        private void Inject(ICounterForShowAd counter) =>
            _counter = counter;

        public void OnAddCounterForShowAd() =>
            _counter.Add();
    }
}