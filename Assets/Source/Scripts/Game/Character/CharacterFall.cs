using BikeDefied.FSM.Game;
using Reflex.Attributes;
using UnityEngine;

namespace BikeDefied.Game.Character
{
    public class CharacterFall : MonoBehaviour
    {
        [SerializeField] private Animator _selfAnimator;

        private IGameOver _game;

        [Inject]
        private void Inject(GameStateInject inject)
        {
            _game = inject.Over;
            _game.GameOver += OnGameOver;
        }

        private void OnEnable()
        {
            if (_game != null)
                _game.GameOver += OnGameOver;
        }

        private void OnDisable()
        {
            _game.GameOver -= OnGameOver;
        }

        private bool OnGameOver()
        {
            _selfAnimator.enabled = false;

            return true;
        }
    }
}