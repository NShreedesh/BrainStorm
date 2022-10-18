using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    private Input input;

    public bool IsPressed { get; private set; }
    public Vector2 Position { get; private set; }

    public Action OnClickStartedAction;
    public Action OnPositionChangedAction;
    public Action OnClickCanceledAction;

    private void OnEnable()
    {
        input = new Input();
        input.Enable();

        input.Player.Click.started += OnClickStarted;
        input.Player.Click.canceled += OnClickCanceled;

        input.Player.Position.performed += OnPositionChanged;
    }

    private void OnClickStarted(InputAction.CallbackContext obj)
    {
        IsPressed = true;
        OnClickStartedAction?.Invoke();
    }

    private void OnClickCanceled(InputAction.CallbackContext obj)
    {
        IsPressed = false;
        OnClickCanceledAction?.Invoke();
    }

    private void OnPositionChanged(InputAction.CallbackContext obj)
    {
        Position = obj.ReadValue<Vector2>();
        OnPositionChangedAction?.Invoke();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
