using Agava.YandexGames;
using System;

public class Ad : IDisposable
{
    private IGameOver _over;
    private readonly int _countOverBetweenShowsAd = 3;
    private int _currentCountOver;

    public Ad(IGameOver over, int countOverBetweenShowsAd)
    {
        _over = over;
        _over.GameOver += OnGameOver;
        _countOverBetweenShowsAd = countOverBetweenShowsAd;
    }

    public void Dispose()
    {
        _over.GameOver -= OnGameOver;
    }

    public void Show()
    {
        VideoAd.Show();
    }

    private void OnGameOver()
    {
        if (++_currentCountOver >= _countOverBetweenShowsAd)
        {
            Show();
            _currentCountOver = 0;
        }
    }
}