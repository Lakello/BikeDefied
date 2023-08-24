using UnityEngine;

[RequireComponent(typeof(GroundChecker))]
public abstract class BikeBehaviour : MonoBehaviour
{
    protected Player Player;
    protected IInputHandler InputHandler;
    protected Coroutine BehaviourCoroutine;

    private GroundChecker _groundChecker;
    private IGameOver _game;

    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        _groundChecker = GetComponent<GroundChecker>();
    }

    private void OnEnable()
    {
        if (_game != null)
            _game.GameOver += OnGameOver;

        _groundChecker.GroundChanged += OnGroundChanged;
    }

    private void OnDisable()
    {
        if (BehaviourCoroutine != null)
            StopCoroutine(BehaviourCoroutine);

        _game.GameOver -= OnGameOver;

        _groundChecker.GroundChanged -= OnGroundChanged;
    }

    protected void Init(BikeBehaviourInject inject)
    {
        InputHandler = inject.Input;

        _game = inject.Game;
        _game.GameOver += OnGameOver;

        Player = inject.Player;
    }

    protected abstract void Inject(BikeBehaviourInject inject);
    protected abstract void OnGameOver();

    private void OnGroundChanged(bool value) => IsGrounded = value;
}