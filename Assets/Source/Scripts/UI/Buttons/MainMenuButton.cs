using Reflex.Attributes;

public class MainMenuButton : EventTriggerButton
{
    private ICounterForShowAd _counter;

    public void OnAddCounterForShowAd() =>
        _counter.Add();

    [Inject]
    private void Inject(ICounterForShowAd counter) =>
        _counter = counter;
}