using Agava.YandexGames;

public class Ad : ICounterForShowAd
{
    private Context _context;
    private readonly int _countOverBetweenShowsAd = 5;
    private int _currentCountOver;

    public Ad(Context context, int countOverBetweenShowsAd)
    {
        _context = context;
        _countOverBetweenShowsAd = countOverBetweenShowsAd;
    }

    public void Add()
    {
        if (++_currentCountOver % _countOverBetweenShowsAd == 0)
            Show();
    }

    private void Show()
    {
#if !UNITY_EDITOR
        InterstitialAd.Show(
            onOpenCallback: () => _context.ChangeFocusAd(false, true),
            onCloseCallback: (wasShown) => _context.ChangeFocusAd(true, false));
#endif
    }
}