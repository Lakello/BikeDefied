using Reflex.Attributes;
using UnityEngine;

namespace BikeDefied.InputSystem
{
    public class DesktopInputHandler : MonoBehaviour, IInputHandler
    {
        private PlayerInput _playerInput;

        public float Horizontal => _playerInput.PC.Horizontal.ReadValue<float>();

        [Inject]
        private void Inject(PlayerInput input) =>
            _playerInput = input;
    }
}