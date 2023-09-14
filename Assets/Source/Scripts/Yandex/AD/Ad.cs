using Agava.YandexGames;
using System;

public class Ad : IDisposable
{
    private IGameOver _over;
    private readonly int _countOverBetweenShowsVideoAd = 10;
    private readonly int _countOverBetweenShowsAd = 3;
    private int _currentCountOver;

    public Ad(IGameOver over, int countOverBetweenShowsAd, int countOverBetweenShowsVideoAd)
    {
        _over = over;
        _over.GameOver += OnGameOver;
        _countOverBetweenShowsAd = countOverBetweenShowsAd;
        _countOverBetweenShowsVideoAd = countOverBetweenShowsVideoAd;
    }

    public void Dispose()
    {
        _over.GameOver -= OnGameOver;
    }

    public void Show()
    {
#if !UNITY_EDITOR
        InterstitialAd.Show();
#endif
    }

    public void ShowVideo()
    {
#if !UNITY_EDITOR
        VideoAd.Show();
#endif
    }

    private void OnGameOver()
    {
        if (++_currentCountOver % _countOverBetweenShowsVideoAd == 0)
        {
            ShowVideo();
        }
        else if (_currentCountOver % _countOverBetweenShowsAd == 0)
        {
            Show();
        }
    }
}