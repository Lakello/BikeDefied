using UnityEngine;

[RequireComponent(typeof(GroundChecker))]
public abstract class BikeBehaviour : MonoBehaviour
{
    [SerializeField] protected Player Player;

    protected IInputHandler InputHandler;
    protected Coroutine BehaviourCoroutine;

    protected bool IsGrounded { get; private set; }
    protected bool IsBackWheelGrounded { get; private set; }

    private GroundChecker _groundChecker;
    private IGameOver _game;

    private void Awake()
    {
        _groundChecker = GetComponent<GroundChecker>();
    }

    private void OnEnable()
    {
        if (_game != null)
            _game.GameOver += OnGameOver;

        _groundChecker.GroundChanged += OnGroundChanged;
        _groundChecker.BackWheelGroundChanged += OnBackWheelGroundChanged;
    }

    private void OnDisable()
    {
        StopCoroutine(BehaviourCoroutine);

        _game.GameOver -= OnGameOver;

        _groundChecker.GroundChanged -= OnGroundChanged;
        _groundChecker.BackWheelGroundChanged -= OnBackWheelGroundChanged;
    }

    protected void Init(IInputHandler input, IGameOver game)
    {
        InputHandler = input;
        _game = game;
        _game.GameOver += OnGameOver;
    }

    protected abstract void Inject(IInputHandler input, IGameOver game);
    protected abstract void OnGameOver();

    private void OnGroundChanged(bool value) => IsGrounded = value;
    private void OnBackWheelGroundChanged(bool value) => IsBackWheelGrounded = value;
}