using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;

    public static Vector2 Movement;
    public static Vector2 Aim;
    public static bool RunIsHeld;

    private InputAction _moveAction;
    private InputAction _directAction;
    private InputAction _runAction;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        _moveAction = PlayerInput.actions["Move"];
        _directAction = PlayerInput.actions["Aim"];
        _runAction = PlayerInput.actions["Run"];
    }

    private void Update()
    {
        Movement = _moveAction.ReadValue<Vector2>();
        Aim = _directAction.ReadValue<Vector2>();

        RunIsHeld = _runAction.IsPressed();
    }
}
