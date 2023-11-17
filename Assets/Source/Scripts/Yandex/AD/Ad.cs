using Agava.YandexGames;

namespace BikeDefied.Yandex.AD
{
    public class Ad : ICounterForShowAd
    {
        private readonly int _countOverBetweenShowsAd = 5;
        
        private FocusObserver _focusObserver;
        private int _currentCountOver;

        public Ad(FocusObserver context, int countOverBetweenShowsAd)
        {
            _focusObserver = context;
            _countOverBetweenShowsAd = countOverBetweenShowsAd;
        }

        public void Add()
        {
            if (++_currentCountOver % _countOverBetweenShowsAd == 0)
            {
                Show();
            }
        }

        private void Show()
        {
#if !UNITY_EDITOR
            InterstitialAd.Show(
                onOpenCallback: () => _focusObserver.ChangeFocusAd(false, true),
                onCloseCallback: (wasShown) => _focusObserver.ChangeFocusAd(true, false));
#endif
        }
    }
}