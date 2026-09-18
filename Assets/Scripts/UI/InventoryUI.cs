using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class InventoryUI : MonoBehaviour
{
    [SerializeField] OptionsBubble optionsBubble;
    public Slot[] slots;
    private Slot selectedSlot;
    private TextMeshProUGUI bubbleCountText;
    private PlayerInputActions inputActions;
    [SerializeField] PlayerController playerController;
    [SerializeField] RectTransform bubbleCountRectTransform;
    private RectTransform rectTransform;
    private Vector2 shownPosition;
    private Vector2 hiddenPosition;
    private Vector2 moveInventoryInput;
    private float inventoryInput;
    private float inventorySelectInput;

    private bool isOpen;
    private bool isAnimated;
    private bool isCooling;
    private bool isSelecting;
    public int selectingRow;
    public int selectingCol;
    private Coroutine currentAnimation;
    private Coroutine currentCooldown;
    
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.OpenInventory.performed += OnInventoryPerformed;
        inputActions.Player.OpenInventory.canceled += OnInventoryCanceled;
        inputActions.Player.MoveInventory.performed += OnMoveInventoryPerformed;
        inputActions.Player.MoveInventory.canceled += OnMoveInventoryCanceled;
        inputActions.Player.Action.performed += OnInventoryActionPerformed;
        inputActions.Player.Action.canceled += OnInventoryActionCanceled;
    }
    private void OnDisable()
    {
        inputActions.Player.OpenInventory.performed -= OnInventoryPerformed;
        inputActions.Player.OpenInventory.canceled -= OnInventoryCanceled;
        inputActions.Player.MoveInventory.performed -= OnMoveInventoryPerformed;
        inputActions.Player.MoveInventory.canceled -= OnMoveInventoryCanceled;
        inputActions.Player.Action.performed -= OnInventoryActionPerformed;
        inputActions.Player.Action.canceled -= OnInventoryActionCanceled;
        inputActions.Player.Disable();
    }
    private void OnInventoryActionPerformed(InputAction.CallbackContext context)
    {
        if (isOpen) inventorySelectInput = context.ReadValue<float>();
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
    void Awake()
    {
        slots = new Slot[10];
        rectTransform = GetComponent<RectTransform>();
        bubbleCountText = GetComponentInChildren<TextMeshProUGUI>();
        inputActions = new PlayerInputActions();
        isAnimated = false;
        isOpen = false;
        isSelecting = false;
        hiddenPosition = new Vector2(0f, -1500f);
        shownPosition = new Vector2(0f, 0f);
        selectedSlot = null;
    }
    void Start()
    {
        rectTransform.anchoredPosition = hiddenPosition;
        selectingCol = 0;
        selectingRow = 0;
        if (playerController == null)
        {
            Debug.Log("InventoryUI reference for playerController is null");
        }
        
        if (optionsBubble == null)
        {
            Debug.Log("InventoryUI reference for OptionsBubble is null");
        }
        bubbleCountRectTransform.anchoredPosition = new Vector2(570f,350f);
        bubbleCountText.text = PlayerStats.Instance.bubbles.ToString();
    }
    void Update()
    {
        // Open inventory when button is pressed
        if (inventoryInput != 0 && !isAnimated && !isSelecting)
        {
            OpenInventory();
        }
        // select slot
        if (!isCooling && isOpen && !isSelecting)
        {
            if (inventorySelectInput != 0)
            {
                if (selectedSlot != null && selectedSlot.itemPrefab != null)
                {
                    //isSelecting = true;
                    ItemData data = selectedSlot.itemData;
                    optionsBubble.SetupOptions(data.options); //Shows up, Displays, and allows movement.
                    // Block inventory movement while option is being selected
                    // move this: selectedSlot.Drop();
                    // to optionsBubble orrrrr, give bubble script access
                    // to call these functions
                    // make this decision
                    // change list of strings to a struct that contains the name of the option
                    // and a reference to the command that slot will execute
                    // InventoryUI is responsible for calling Slot for funcs
                }
                isCooling = true;
                if (currentCooldown != null) StopCoroutine(currentCooldown);
                currentCooldown = StartCoroutine(Cooldown());
            }
        }
        if(!isCooling && (moveInventoryInput.y != 0 || moveInventoryInput.x != 0))
        {
            HoverSlot();
        }
    }
    public void CallFunction()
    {
        //selectedSlot.
        isSelecting = false;
    }
    // totally necessary
    private void HoverSlot()
    {
        if (selectedSlot != null)
        {
            selectedSlot.QuitHover();
        }
        isCooling = true;
        // COL
        int sum = (int)moveInventoryInput.x + selectingCol;
        if (sum < 0) selectingCol = 4;
        else if (sum > 4) selectingCol = 0;
        else selectingCol = sum;
        // ROW
        sum = (int)moveInventoryInput.y + selectingRow;
        if (sum < 0) selectingRow = 1;
        else if (sum > 1) selectingRow = 0;
        else selectingRow = sum;
        selectedSlot = slots[(5 * selectingRow) + selectingCol];
        selectedSlot.Hover();
        if (currentCooldown != null) StopCoroutine(currentCooldown);
        currentCooldown = StartCoroutine(Cooldown());
    }
    private int FindAvailableSlot()
    {
        if (slots[(5*selectingRow) + selectingCol].icon == null) return (5*selectingRow) + selectingCol; 
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].icon == null) return i;
        }
        return -1;
    }
    //opened by dialogue system or by Player when dealing
    public void OpenInventory()
    {
        if (selectedSlot == null)
        {
            selectedSlot = slots[0];
            selectedSlot.Hover();
        }
        isAnimated = true; // Necessary
        bubbleCountText.text = PlayerStats.Instance.bubbles.ToString(); // Necessary
        Vector2 target = isOpen ? hiddenPosition : shownPosition; // Necessary
        if (isOpen && selectedSlot.itemPrefab != null)
        {
            selectedSlot.QuitHover();
        }
        isOpen = !isOpen; // Necessary
        playerController.isInteracting = isOpen; // Necessary
        if (currentAnimation != null) StopCoroutine(currentAnimation);
        currentAnimation = StartCoroutine(AnimatePanel(target));
    }
    // this function is totally independent
    private System.Collections.IEnumerator AnimatePanel(Vector2 target)
    {
        Vector2 start = rectTransform.anchoredPosition;
        float elapsed = 0f;
        
        while (elapsed < 0.3f)
        {
            elapsed += Time.deltaTime;
            float tiempo = elapsed / 0.3f;
            rectTransform.anchoredPosition = Vector2.Lerp(start, target, tiempo);
            yield return null;
        }
        rectTransform.anchoredPosition = target;
        isAnimated = false;
    }
    // independent function
    private System.Collections.IEnumerator Cooldown()
    {
        float elaps = 0f;
        while (elaps < 0.3f)
        {
            elaps += Time.deltaTime;
            float tiempo = elaps / 0.3f;
            yield return null;
        }
        isCooling = false;
    }
    public bool Add(Item item)
    {
        int nextAvailableSlot = FindAvailableSlot();
        if (nextAvailableSlot != -1)
        {
            slots[nextAvailableSlot].SetContainer(item.data);
            return true;
        }
        else
        {
            Debug.Log("the inventory is full");
            return false;
        }
    }
}
