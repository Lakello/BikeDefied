using UnityEngine;

public class GameInit : MonoBehaviour
{
    //private void OnEnable() => YandexGame.GetDataEvent += EndInit;

    //private void OnDisable() => YandexGame.GetDataEvent -= EndInit;

    private void EndInit() => IJunior.TypedScenes.Game.Load<MenuState>();
}
