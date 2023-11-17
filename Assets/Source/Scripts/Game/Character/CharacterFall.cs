using BikeDefied.FSM.Game;
using Reflex.Attributes;
using UnityEngine;

namespace BikeDefied.Game.Character
{
    public class CharacterFall : MonoBehaviour
    {
        [SerializeField] private Animator _selfAnimator;

        private IEndLevelStateChangeble _endLevel;

        [Inject]
        private void Inject(GameStateInject inject)
        {
            _endLevel = inject.EndLevel;
            _endLevel.StateChanged += OnStateChanged;
        }

        private void OnEnable()
        {
            if (_endLevel != null)
            {
                _endLevel.StateChanged += OnStateChanged;
            }
        }

        private void OnDisable() =>
            _endLevel.StateChanged -= OnStateChanged;

        private bool OnStateChanged()
        {
            _selfAnimator.enabled = false;

            return true;
        }
    }
}