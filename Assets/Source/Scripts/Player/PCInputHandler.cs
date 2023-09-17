using Reflex.Attributes;
using UnityEngine;

public class PCInputHandler : MonoBehaviour, IInputHandler
{
    private PlayerInput _playerInput;

    public float Horizontal => _playerInput.PC.Horizontal.ReadValue<float>();

    [Inject]
    private void Inject(PlayerInput input)
    {
        _playerInput = input;
    }
}
