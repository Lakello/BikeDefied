using UnityEngine;

[RequireComponent(typeof(GroundChecker))]
public abstract class BikeBehaviour : MonoBehaviour
{
    protected Player Player;
    protected IInputHandler InputHandler;
    protected Coroutine BehaviourCoroutine;
    protected Transform BikeBody;

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
        if (BehaviourCoroutine != null)
            StopCoroutine(BehaviourCoroutine);

        _game.GameOver -= OnGameOver;

        _groundChecker.GroundChanged -= OnGroundChanged;
        _groundChecker.BackWheelGroundChanged -= OnBackWheelGroundChanged;
    }

    protected void Init(BikeBehaviourInject inject)
    {
        InputHandler = inject.Input;

        _game = inject.Game;
        _game.GameOver += OnGameOver;

        Player = inject.Player;

        BikeBody = inject.BikeBody;
    }

    protected abstract void Inject(BikeBehaviourInject inject);
    protected abstract void OnGameOver();

    private void OnGroundChanged(bool value) => IsGrounded = value;
    private void OnBackWheelGroundChanged(bool value) => IsBackWheelGrounded = value;
}