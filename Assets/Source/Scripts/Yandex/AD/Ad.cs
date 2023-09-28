using Agava.YandexGames;
using System;

public class Ad : IDisposable
{
    private Context _context;
    private IGameOver _over;
    private readonly int _countOverBetweenShowsAd = 5;
    private int _currentCountOver;

    public Ad(Context context, IGameOver over, int countOverBetweenShowsAd)
    {
        _context = context;
        _over = over;
        _over.LateGameOver += OnGameOver;
        _countOverBetweenShowsAd = countOverBetweenShowsAd;
    }

    public void Dispose()
    {
        _over.LateGameOver -= OnGameOver;
    }

    public void Show()
    {
#if !UNITY_EDITOR
        InterstitialAd.Show(
            onOpenCallback: () => _context.ChangeFocusAd(false, true),
            onCloseCallback: (wasShown) => _context.ChangeFocusAd(true, false));
#endif
    }

    private void OnGameOver()
    {
        if (++_currentCountOver % _countOverBetweenShowsAd == 0)
        {
            Show();
        }
    }
}