using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    public float currentSpeed;
    public float pickInput;
    public float buttonInput;
    private bool isMoving;
    public bool isInteracting;
    private bool isDealing;
    private PlayerInputActions inputActions;
    private PlayerStats stats;
    private Vector2 moveInput;
    private Vector2 rotateInput;
    private bool _isInventoryOpen;
    public bool isInventoryOpen { get { return _isInventoryOpen; } set { _isInventoryOpen = value; }}
    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
        inputActions.Player.Rotate.performed += OnRotatePerformed;
        inputActions.Player.Rotate.canceled += OnRotateCanceled;
        inputActions.Player.Pick.performed += OnPickPerformed;
        inputActions.Player.Pick.canceled += OnPickCanceled;
        inputActions.Player.Action.performed += OnActionPerformed;
        inputActions.Player.Action.canceled += OnActionCanceled;
    }
    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Player.Rotate.performed -= OnRotatePerformed;
        inputActions.Player.Rotate.canceled -= OnRotateCanceled;
        inputActions.Player.Pick.performed -= OnPickPerformed;
        inputActions.Player.Pick.canceled -= OnPickCanceled;
        inputActions.Player.Action.performed -= OnActionPerformed;
        inputActions.Player.Action.canceled -= OnActionCanceled;
        inputActions.Player.Disable();
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
        if (!isInteracting)
        {
            isMoving = true;
            moveInput = context.ReadValue<Vector2>();
            currentSpeed = Mathf.Sqrt(((moveInput.x * moveInput.x) + (moveInput.y * moveInput.y)) * speed);
        }
    }
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        isMoving = false;
        moveInput = Vector2.zero;
        currentSpeed = 0f;
    }
    private void OnRotatePerformed(InputAction.CallbackContext context)
    {
        if (!isInteracting) rotateInput = context.ReadValue<Vector2>();
    }
    private void OnRotateCanceled(InputAction.CallbackContext context)
    {
        rotateInput = Vector2.zero;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Bubbles"))
        {
            if (pickInput != 0)
            {
                Bubbles bubbles = other.gameObject.GetComponent<Bubbles>();
                bubbles.Pick();
            }
        }
        if (other.CompareTag("Items"))
        {
            if (pickInput != 0)
            {
                Item item = other.gameObject.GetComponent<Item>();
                item.Pick();
            }
        }
    }
    void Start()
    {
        stats = gameObject.GetComponent<PlayerStats>();
    }
    void Update()
    {
        if (!isInteracting && !isInventoryOpen)
        {
            transform.position += new Vector3(moveInput.x * speed * Time.deltaTime, 0.0f, moveInput.y * speed * Time.deltaTime);

            if (isMoving)
            {
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, Mathf.Atan2(rotateInput.y * -1f,rotateInput.x) * Mathf.Rad2Deg, 0));

                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    1080f * Time.deltaTime);   
            }
        }
    }
}
