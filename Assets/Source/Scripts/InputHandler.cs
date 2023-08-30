using UnityEngine;

public class InputHandler
{
    private static InputHandler _instance;

    private PlayerInput _input;

    public static InputHandler Instance => _instance;

    public PlayerInput Input => _input;

    public InputHandler Init()
    {
        if (_instance == null)
        {
            _instance = this;
            _input = new PlayerInput();
        }


        return _instance;
    }
}