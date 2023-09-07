using Reflex.Attributes;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _lookAt;
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;

    private IGameOver _game;

    private void OnEnable()
    {
        if (_game != null)
            _game.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        _game.GameOver -= OnGameOver;
    }

    private void Update()
    {
        transform.position = _target.position + _offset;
    }

    [Inject]
    private void Inject(GameStateInject inject)
    {
        _game = inject.Over;
        _game.GameOver += OnGameOver;
    }

    private void OnGameOver()
    {
        enabled = false;
        _lookAt.parent = null;
    }
}