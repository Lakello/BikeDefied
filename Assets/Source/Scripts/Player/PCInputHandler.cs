using Reflex.Attributes;
using Unity.VisualScripting;
using UnityEngine;

public class PCInputHandler : MonoBehaviour, IInputHandler
{
    private PlayerInput _playerInput;

    public float Horizontal => _playerInput.PC.Horizontal.ReadValue<float>();

    public PCInputHandler(PlayerInput input)
    {
        _playerInput = input;
    }
}
