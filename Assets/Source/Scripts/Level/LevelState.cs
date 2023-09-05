using UnityEngine;

public class LevelState
{
    private Level _level;
    private FinishPoint _point;
    private Finish _finish;

    public LevelState(Level levelPrefab, GameObject parent, Finish finish)
    {
        _level = Object.Instantiate(levelPrefab, parent.transform);
        _level.gameObject.SetActive(false);
        _finish = finish;

        _point = _level.GetComponentInChildren<FinishPoint>();
    }

    public void Enter()
    {
        _level.gameObject.SetActive(true);
        _finish.OnPointEnabled(_point.transform.position);
    }

    public void Exit() 
    {
        _level.gameObject.SetActive(false);
    }
}