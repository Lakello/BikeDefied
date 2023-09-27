using IJunior.StateMachine;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvas : MonoBehaviour
{
    [SerializeField] private List<Window> _tutorialWindows;

    private int _currentIndex;
    private Window _currentWindow;

    private void Awake()
    {
        _currentIndex = 0;

        UpdateWindow();
    }

    public void NextWindow()
    {
        if (_currentIndex + 1 >= _tutorialWindows.Count)
            return;

        _currentIndex++;

        UpdateWindow();
    }

    public void Exit()
    {
        Destroy(gameObject);
    }

    private void UpdateWindow()
    {
        _currentWindow?.gameObject.SetActive(false);
        _currentWindow = _tutorialWindows[_currentIndex];
        _currentWindow.gameObject.SetActive(true);
    }
}