using UnityEngine;
using UnityEngine.InputSystem;

public class RegularDialogueBubble : DialogueBubble
{
    private PlayerInputActions inputActions;
    private float selectInput;
    private Vector2 moveInput;
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.MoveInventory.performed += OnMovePerformed;
        inputActions.Player.MoveInventory.canceled += OnMoveCanceled;
        inputActions.Player.Action.performed += OnSelectPerformed;
        inputActions.Player.Action.canceled += OnSelectCanceled;
    }
    private void OnDisable()
    {
        inputActions.Player.MoveInventory.performed -= OnMovePerformed;
        inputActions.Player.MoveInventory.canceled -= OnMoveCanceled;
        inputActions.Player.Action.performed -= OnSelectPerformed;
        inputActions.Player.Action.canceled -= OnSelectCanceled;
        inputActions.Player.Disable();
    }
    private void OnSelectPerformed(InputAction.CallbackContext context)
    {
        selectInput = context.ReadValue<float>();
    }
    private void OnSelectCanceled(InputAction.CallbackContext context)
    {
        selectInput = 0f;
    }
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = new Vector2(0,0);
    }
    public void SetText(string content)
    {
        text.text = content;
    }
    public void AdvanceOrClose()
    {

    }
}
