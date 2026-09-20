using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;
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
    public Vector2 moveInventoryInput;
    private float inventoryInput;
    public float inventorySelectInput;

    private bool isOpen;
    private bool isAnimated;
    public bool isCooling;
    public bool isSelecting;
    public bool isSelling;
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
        // OPEN AND CLOSE INVENTORY
        if (inventoryInput != 0 && !isAnimated && !isSelecting && !isSelling)
        {
            OpenInventory();
        }
        // SELECT AND MOVE THROUGH SLOTS
        if (!isCooling && isOpen && !isSelecting)
        {
            if(moveInventoryInput.y != 0 || moveInventoryInput.x != 0)
            {
                HoverSlot();
            }
            if (inventorySelectInput != 0)
            {
                if (isSelling)
                {
                    if (selectedSlot != null && selectedSlot.itemPrefab != null)
                    {
                        if (Sell())
                        {
                            OpenInventory();
                            isSelling = false;
                        }
                    }
                }
                else
                {
                    if (selectedSlot != null && selectedSlot.itemPrefab != null)
                    {
                        isSelecting = true;
                        ShowOptionsFor(selectedSlot.itemData);
                    }
                    isCooling = true;
                    if (currentCooldown != null) StopCoroutine(currentCooldown);
                    currentCooldown = StartCoroutine(Cooldown());
                }
            }
        }
    }
    public void ShowOptionsFor(ItemData data)
    {
        List<DialogueOption> resolvedOptions = new List<DialogueOption>();

        foreach (string optionText in data.options)
        {
            System.Action action = optionText switch
            {
                "Use" => selectedSlot.Use,
                "Eat" => Eat,
                "Give" => selectedSlot.Give,
                _ => null
            };

            resolvedOptions.Add(new DialogueOption { text = optionText, onOptionSelected = action});
        }
        resolvedOptions.Add(new DialogueOption { text = "Swap", onOptionSelected = Swap});
        resolvedOptions.Add(new DialogueOption { text = "Drop", onOptionSelected = selectedSlot.Drop});

        optionsBubble.SetupOptions(resolvedOptions, selectedSlot.GetComponent<RectTransform>().anchoredPosition);
    }
    // totally necessary
    private void HoverSlot()
    {
        if (selectedSlot != null)
        {
            selectedSlot.QuitHover();
        }
        isCooling = true;
        selectingCol += (int)moveInventoryInput.x;
        selectingCol = (selectingCol < 0) ? 4 : selectingCol % 5;
        selectingRow += (int)moveInventoryInput.y;
        selectingRow = (selectingRow < 0) ? 1 : selectingRow % 2;
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
        }
        isAnimated = true;
        bubbleCountText.text = PlayerStats.Instance.bubbles.ToString();
        Vector2 target = isOpen ? hiddenPosition : shownPosition;
        isOpen = !isOpen;
        selectedSlot.Hover();
        playerController.isInteracting = isOpen;
        if (!isOpen)
        {
            if (selectedSlot.itemPrefab != null) selectedSlot.QuitHover();
        }
        else inventorySelectInput = 0f;
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
    public bool Sell()
    {
        Debug.Log("Sell");
        if (currentCooldown != null) StopCoroutine(currentCooldown);
        currentCooldown = StartCoroutine(Cooldown());
        if (!selectedSlot.itemData.isSaleable) return false;
        PlayerStats.Instance.bubbles += selectedSlot.itemData.price;
        selectedSlot.Discard();
        return true;
    }
    public void Eat()
    {
        Debug.Log("Eat");
        if (currentCooldown != null) StopCoroutine(currentCooldown);
        currentCooldown = StartCoroutine(Cooldown());
        selectedSlot.Discard();
    }
    public void Swap()
    {
        Debug.Log("Swap");
        // Volver a seleccionar con quien se pretenda intercambiar
    }
}
