using UnityEngine;

public class LevelState
{
    private Level _level;

    public LevelState(Level levelPrefab, GameObject parent)
    {
        _level = Object.Instantiate(levelPrefab, parent.transform);
        _level.gameObject.SetActive(false);
    }

    public void Enter()
    {
        _level.gameObject.SetActive(true);
    }

    public void Exit() 
    {
        _level.gameObject.SetActive(false);
    }
}