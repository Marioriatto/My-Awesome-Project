using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;
// singleton
public class InputActions : MonoBehaviour
{
    public static InputActions Instance { get; private set;}
    private PlayerInputActions actions;
    public Vector2 moveInventoryInput, moveInput, rotateInput;
    public float inventorySelectInput, inventoryInput, buttonInput, pickInput, actionInput;
    public bool isMoving, isOpen;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        actions = new PlayerInputActions();
    }
    
    private void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Move.performed += OnMovePerformed;
        actions.Player.Move.canceled += OnMoveCanceled;
        actions.Player.Rotate.performed += OnRotatePerformed;
        actions.Player.Rotate.canceled += OnRotateCanceled;
        actions.Player.Pick.performed += OnPickPerformed;
        actions.Player.Pick.canceled += OnPickCanceled;
        actions.Player.Action.performed += OnActionPerformed;
        actions.Player.Action.canceled += OnActionCanceled;
        actions.Player.OpenInventory.performed += OnInventoryPerformed;
        actions.Player.OpenInventory.canceled += OnInventoryCanceled;
        actions.Player.Action.performed += OnInventoryActionPerformed;
        actions.Player.Action.canceled += OnInventoryActionCanceled;
        actions.Player.MoveInventory.performed += OnMoveInventoryPerformed;
        actions.Player.MoveInventory.canceled += OnMoveInventoryCanceled;
    }
    private void OnDisable()
    {
        actions.Player.Move.performed -= OnMovePerformed;
        actions.Player.Move.canceled -= OnMoveCanceled;
        actions.Player.Rotate.performed -= OnRotatePerformed;
        actions.Player.Rotate.canceled -= OnRotateCanceled;
        actions.Player.Pick.performed -= OnPickPerformed;
        actions.Player.Pick.canceled -= OnPickCanceled;
        actions.Player.Action.performed -= OnActionPerformed;
        actions.Player.Action.canceled -= OnActionCanceled;
        actions.Player.OpenInventory.performed -= OnInventoryPerformed;
        actions.Player.OpenInventory.canceled -= OnInventoryCanceled;
        actions.Player.MoveInventory.performed -= OnMoveInventoryPerformed;
        actions.Player.MoveInventory.canceled -= OnMoveInventoryCanceled;
        actions.Player.Action.performed -= OnInventoryActionPerformed;
        actions.Player.Action.canceled -= OnInventoryActionCanceled;
        actions.Player.Disable();
    }
    private void OnActionPerformed(InputAction.CallbackContext context)
    {
        buttonInput = context.ReadValue<float>();
    }
    private void OnActionCanceled(InputAction.CallbackContext context)
    {
        buttonInput = 0f;
    }
    private void OnPickPerformed(InputAction.CallbackContext context)
    {
        pickInput = context.ReadValue<float>();
    }
    private void OnPickCanceled(InputAction.CallbackContext context)
    {
        pickInput = 0f;
    }
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        isMoving = true;
        moveInput = context.ReadValue<Vector2>();
    }
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        isMoving = false;
        moveInput = Vector2.zero;
    }
    private void OnRotatePerformed(InputAction.CallbackContext context)
    {
        rotateInput = context.ReadValue<Vector2>();
    }
    private void OnRotateCanceled(InputAction.CallbackContext context)
    {
        rotateInput = Vector2.zero;
    }
    private void OnInventoryActionPerformed(InputAction.CallbackContext context)
    {
        inventorySelectInput = context.ReadValue<float>();
    }
    private void OnInventoryActionCanceled(InputAction.CallbackContext context)
    {
        inventorySelectInput = 0f;
    }
    private void OnMoveInventoryPerformed(InputAction.CallbackContext context)
    {
        if (isOpen) moveInventoryInput = context.ReadValue<Vector2>();
    }
    private void OnMoveInventoryCanceled(InputAction.CallbackContext context)
    {
        moveInventoryInput = new Vector2(0,0);
    }
    private void OnInventoryPerformed(InputAction.CallbackContext context)
    {
        inventoryInput = context.ReadValue<float>();
    }
    private void OnInventoryCanceled(InputAction.CallbackContext context)
    {
        inventoryInput = 0f;
    }
}