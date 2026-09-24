using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class InventoryUI : MonoBehaviour
{
    [SerializeField] OptionsBubble optionsBubble;
    public Slot[] slots;
    private Slot selectedSlot, swappingSlot;
    private TextMeshProUGUI bubbleCountText;
    [SerializeField] PlayerController playerController;
    [SerializeField] RegularDialogueBubble regularDialogueBubble;
    [SerializeField] RectTransform bubbleCountRectTransform;
    private RectTransform rectTransform;
    private Vector2 shownPosition, hiddenPosition;
    private bool isSwappingSlots, isInventoryPanelAnimated;
    private int swappingRow, swappingCol;
    public bool isCoolingAction, isSelectingSlotOptions, isSelling, isPlayerInventory;
    public int selectingRow, selectingCol;
    private Coroutine currentAnimatePanel, currentCooldown;
    [System.NonSerialized] public Coroutine currentAnimation;
    
    void Awake()
    {
        slots = new Slot[10];
        rectTransform = GetComponent<RectTransform>();
        bubbleCountText = GetComponentInChildren<TextMeshProUGUI>();
        isInventoryPanelAnimated = false;
        isSelectingSlotOptions = false;
        isSwappingSlots = false;
        isSelling = false;
        hiddenPosition = new Vector2(0f, -1500f);
        shownPosition = new Vector2(0f, 0f);
        selectedSlot = null;
        swappingSlot = null;
    }
    void Start()
    {
        InputActions.Instance.isInventoryOpen = false;
        rectTransform.anchoredPosition = hiddenPosition;
        selectingCol = 0;
        selectingRow = 0;
        swappingRow = 0;
        swappingCol = 0;
        if (isPlayerInventory)
        {
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
    }
    void Update()
    {
        // isSwappingSlots is only on player
        // isSelectingSlotOptions is only on player
        // isCoolingAction and isInventoryOpen are independent
        if (InputActions.Instance.isInventoryOpen && !isCoolingAction)
        {
            if(InputActions.Instance.moveInventoryInput.y != 0 || InputActions.Instance.moveInventoryInput.x != 0)
            {
                if (isPlayerInventory)
                {
                    if (!isSelectingSlotOptions)
                        HoverSlot();
                }
                else
                    HoverSlot();
            }
            if (InputActions.Instance.inventorySelectInput != 0)
            {
                if (isPlayerInventory && !isSelectingSlotOptions)
                {
                    if (isSelling)
                    {
                        if (selectedSlot != null && selectedSlot.itemPrefab != null)
                        {
                            if (SellSlot())
                            {
                                OpenInventory();
                                isInventoryPanelAnimated = true;
                            }
                        }
                    }
                    else
                    {
                        if (selectedSlot != null && selectedSlot.itemPrefab != null)
                        {
                            isSelectingSlotOptions = true;
                            ShowOptionsFor(selectedSlot.itemData);
                        }
                        isCoolingAction = true;
                        if (currentCooldown != null) StopCoroutine(currentCooldown);
                        currentCooldown = StartCoroutine(Cooldown());
                    }
                }
                // code:
                // lo que quiero que haga cuando clicqueo el slot en un inventario distinto
                // al del jugador
            }
            if (isPlayerInventory)
            {
                // Swap
                if(isSelectingSlotOptions && isSwappingSlots)
                {
                    // Move
                    if(InputActions.Instance.moveInventoryInput.y != 0 || InputActions.Instance.moveInventoryInput.x != 0)
                    {
                        HoverSlot();
                    }
                    // Select swap slot
                    if (InputActions.Instance.inventorySelectInput != 0)
                    {
                        if (swappingSlot != null)
                        {
                            ItemData temp = null;
                            selectedSlot.StopGlow();
                            if (swappingSlot.itemData != null)
                            {
                                temp = swappingSlot.itemData;
                            }
                            swappingSlot.Discard();
                            swappingSlot.SetContainer(selectedSlot.itemData);
                            selectedSlot.Discard();
                            if (temp != null) selectedSlot.SetContainer(temp);
                            isSelectingSlotOptions = false;
                            isSwappingSlots = false;
                            selectingCol = swappingCol;
                            selectingRow = swappingRow;
                            selectedSlot = swappingSlot;
                            swappingSlot = null;
                            swappingCol = 0;
                            swappingRow = 0;
                            selectedSlot.Hover();
                        }
                        isCoolingAction = true;
                        if (currentCooldown != null) StopCoroutine(currentCooldown);
                        currentCooldown = StartCoroutine(Cooldown());
                    }
                } 
            }
        }
        if (!isInventoryPanelAnimated)
        {
            if (isPlayerInventory && !isSelectingSlotOptions)
            {
                if (isSelling)
                {
                    if (InputActions.Instance.pickInput != 0 && InputActions.Instance.isInventoryOpen)
                        OpenInventory();
                }
                else
                    if (InputActions.Instance.inventoryInput != 0)
                        OpenInventory();
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
                "Use" => Use,
                "Eat" => Eat,
                "Give" => Give,
                _ => null
            };

            resolvedOptions.Add(new DialogueOption { text = optionText, onOptionSelected = action});
        }
        resolvedOptions.Add(new DialogueOption { text = "Swap", onOptionSelected = Swap});
        resolvedOptions.Add(new DialogueOption { text = "Drop", onOptionSelected = Drop});

        optionsBubble.SetupOptions(resolvedOptions, selectedSlot.GetComponent<RectTransform>().anchoredPosition);
    }
    
    private void HoverSlot()
    {
        if (isSwappingSlots)
        {
            if (swappingSlot != null)
            {
                swappingSlot.QuitHover();
            }
            isCoolingAction = true;
            swappingCol += (int)InputActions.Instance.moveInventoryInput.x;
            swappingCol = (swappingCol < 0) ? 4 : swappingCol % 5;
            swappingRow += (int)InputActions.Instance.moveInventoryInput.y;
            swappingRow = (swappingRow < 0) ? 1 : swappingRow % 2;
            swappingSlot = slots[(5 * swappingRow) + swappingCol];
            swappingSlot.Hover();
        }
        else
        {
            if (selectedSlot != null)
            {
                selectedSlot.QuitHover();
            }
            isCoolingAction = true;
            selectingCol += (int)InputActions.Instance.moveInventoryInput.x;
            selectingCol = (selectingCol < 0) ? 4 : selectingCol % 5;
            selectingRow += (int)InputActions.Instance.moveInventoryInput.y;
            selectingRow = (selectingRow < 0) ? 1 : selectingRow % 2;
            selectedSlot = slots[(5 * selectingRow) + selectingCol];
            selectedSlot.Hover();
        }
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
    public void OpenInventory()
    {
        if (selectedSlot == null)
        {
            selectedSlot = slots[0];
        }
        isInventoryPanelAnimated = true;
        if (!InputActions.Instance.isInventoryOpen)
            bubbleCountText.text = PlayerStats.Instance.bubbles.ToString();
        Vector2 target = InputActions.Instance.isInventoryOpen ? hiddenPosition : shownPosition;
        InputActions.Instance.isInventoryOpen = !InputActions.Instance.isInventoryOpen;
        selectedSlot.Hover();
        if (!isSelling)
            playerController.isInteracting = InputActions.Instance.isInventoryOpen;
        // if isSelling, regularDialogueBubble will turn it off
        if (!InputActions.Instance.isInventoryOpen)
        {
            if (selectedSlot.itemPrefab != null) selectedSlot.QuitHover();
        }
        else InputActions.Instance.inventorySelectInput = 0f;
        if (currentAnimatePanel != null) StopCoroutine(currentAnimatePanel);
        currentAnimatePanel = StartCoroutine(AnimatePanel(target));
    }
    private System.Collections.IEnumerator AnimatePanel(Vector2 target)
    {
        while (currentAnimation != null)
            yield return null;
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
        isInventoryPanelAnimated = false;
        if (!InputActions.Instance.isInventoryOpen)
        {
            if (regularDialogueBubble.isChoosing)
            {
                if (regularDialogueBubble.currentAnimation != null ) StopCoroutine(regularDialogueBubble.currentAnimation);
                currentAnimation = StartCoroutine(regularDialogueBubble.AnimatePanel(regularDialogueBubble.shownPosition));
            }
            isSelling = false;
        }
    }
    
    private System.Collections.IEnumerator Cooldown()
    {
        float elaps = 0f;
        while (elaps < 0.3f)
        {
            elaps += Time.deltaTime;
            yield return null;
        }
        isCoolingAction = false;
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
    public void Sell()
    {
        // allow to sell one or more items instead
        isSelling = true;
        if (InputActions.Instance.isInventoryOpen || isInventoryPanelAnimated || isSelectingSlotOptions) 
        {
            isSelling = false;
            return;
        }
        StartCoroutine(regularDialogueBubble.AnimatePanel(regularDialogueBubble.hiddenPosition));
        OpenInventory();
    }
    private bool SellSlot()
    {
        if (currentCooldown != null) StopCoroutine(currentCooldown);
        currentCooldown = StartCoroutine(Cooldown());
        if (!selectedSlot.itemData.isSaleable) return false;
        int startAmount = PlayerStats.Instance.bubbles;
        if (PlayerStats.Instance.ChangeBubbles(selectedSlot.itemData.price))
        {
            selectedSlot.Discard();
            if (currentAnimation != null) StopCoroutine(currentAnimation);
            currentAnimation = StartCoroutine(BubblesAnimation(startAmount));
            return true;   
        }
        else
            return false;
    }
    public void Use()
    {
        selectedSlot.Use();
    }
    public void Give()
    {
        selectedSlot.Give();
    }
    public void Eat()
    {
        if (currentCooldown != null) StopCoroutine(currentCooldown);
        currentCooldown = StartCoroutine(Cooldown());
        selectedSlot.Discard();
        isSelectingSlotOptions = false;
    }
    public void Swap()
    {
        isSwappingSlots = true;
        selectedSlot.StartGlow();
        isCoolingAction = true;
        swappingCol = selectingCol;
        swappingRow = selectingRow;
        if(currentCooldown != null) StopCoroutine(currentCooldown);
        currentCooldown = StartCoroutine(Cooldown());
    }
    public void Drop()
    {
        selectedSlot.Drop();
        isSelectingSlotOptions = false;
    }
    private System.Collections.IEnumerator BubblesAnimation(int amount)
    {
        int startAmount = amount;
    
        while (startAmount <= PlayerStats.Instance.bubbles)
        {
            bubbleCountText.text = startAmount.ToString();
            startAmount += 10;
            yield return null;
        }
        bubbleCountText.text = PlayerStats.Instance.bubbles.ToString();
        currentAnimation = null;
    }
}