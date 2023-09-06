using Reflex.Attributes;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BikePhysicsBehaviour : MonoBehaviour
{
    [SerializeField] private float _maxGroundMassCenter;
    [SerializeField] private float _minGroundMassCenter;
    [SerializeField] private float _maxFlyMassCenter;
    [SerializeField] private float _minFlymassCenter;
    [SerializeField] private float _maxVelocityForMaxMassCenter;

    private IGamePlay _play;
    private IGameOver _over;
    private GroundChecker _checker;

    private Coroutine _movePhysicsCoroutine;
    private Rigidbody _rigidbody;
    private bool _isAlive;
    private bool _isGrounded;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _isAlive = true;

        Check.IfNotNullThen(_play, () => _play.GamePlay += OnGamePlay);
        Check.IfNotNullThen(_checker, () => _checker.GroundChanged += OnGroundChanged);
    }

    private void OnDisable()
    {
        Check.IfNotNullThen(_play, () => _play.GamePlay -= OnGamePlay);
        Check.IfNotNullThen(_checker, () => _checker.GroundChanged -= OnGroundChanged);
        Check.IfNotNullThen(_movePhysicsCoroutine, () => StopCoroutine(_movePhysicsCoroutine));
    }

    [Inject]
    private void Inject(IGamePlay gamePlay, IGameOver gameOver)
    {
        _play = gamePlay;
        _over = gameOver;
        _play.GamePlay += OnGamePlay;
        _over.GameOver += () => _isAlive = false;
    }

    [Inject]
    private void Inject(GroundChecker checker)
    {
        _checker = checker;
        _checker.GroundChanged += OnGroundChanged;
    }

    private void OnGroundChanged(bool value) => _isGrounded = value;

    private void OnGamePlay()
    {
        _rigidbody.isKinematic = false;
        _movePhysicsCoroutine = StartCoroutine(PhysicsBehaviour());
    }

    private IEnumerator PhysicsBehaviour()
    {
        while (_isAlive)
        {
            if (_isGrounded)
            {
                _rigidbody.centerOfMass = new Vector3(0, CalculateMassCenter(_maxGroundMassCenter, _minGroundMassCenter), 0);
            }
            else
            {
                _rigidbody.centerOfMass = new Vector3(0, CalculateMassCenter(_maxFlyMassCenter, _minFlymassCenter), 0);
            }

            yield return null;
        }
    }

    private float CalculateMassCenter(float max, float min)
    {
        var current = Mathf.Abs(_rigidbody.centerOfMass.y);
        var target = Mathf.Abs(_rigidbody.velocity.z) > 1f ? max : min;
        var time = Mathf.Abs(_rigidbody.velocity.z) / _maxVelocityForMaxMassCenter;

        return -Mathf.Lerp(current, target, time);
    }
}
