using Reflex.Attributes;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BikePhysicsBehaviour : MonoBehaviour
{
    [SerializeField] private float _minMassCenter;

    private Rigidbody _rigidbody;
    private IGamePlay _play;
    private GroundChecker _checker;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (_play != null)
            _play.GamePlay += OnGamePlay;
    }

    private void OnDisable()
    {
        if (_play != null)
            _play.GamePlay -= OnGamePlay;
    }

    [Inject]
    private void Inject(IGamePlay gamePlay)
    {
        _play = gamePlay;
        _play.GamePlay += OnGamePlay;
    }

    [Inject]
    private void Inject(GroundChecker checker)
    {
        _checker = checker;
    }

    private void OnGamePlay()
    {
        _rigidbody.isKinematic = false;
    }
}
