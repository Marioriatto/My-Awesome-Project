using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    private bool isSwapping, isAnimated;
    private int swappingRow, swappingCol;
    public bool isCooling, isSelecting, isSelling;
    public int selectingRow, selectingCol;
    private Coroutine currentAnimation, currentCooldown;
    
    void Awake()
    {
        slots = new Slot[10];
        rectTransform = GetComponent<RectTransform>();
        bubbleCountText = GetComponentInChildren<TextMeshProUGUI>();
        isAnimated = false;
        isSelecting = false;
        isSwapping = false;
        isSelling = false;
        hiddenPosition = new Vector2(0f, -1500f);
        shownPosition = new Vector2(0f, 0f);
        selectedSlot = null;
        swappingSlot = null;
    }
    void Start()
    {
        InputActions.Instance.isOpen = false;
        rectTransform.anchoredPosition = hiddenPosition;
        selectingCol = 0;
        selectingRow = 0;
        swappingRow = 0;
        swappingCol = 0;
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
        if (InputActions.Instance.isOpen && isSelecting && isSwapping && !isCooling)
        {
            if(InputActions.Instance.moveInventoryInput.y != 0 || InputActions.Instance.moveInventoryInput.x != 0)
            {
                HoverSlot();
            }
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
                    isSelecting = false;
                    isSwapping = false;
                    selectingCol = swappingCol;
                    selectingRow = swappingRow;
                    selectedSlot = swappingSlot;
                    swappingSlot = null;
                    swappingCol = 0;
                    swappingRow = 0;
                    selectedSlot.Hover();
                }
                isCooling = true;
                if (currentCooldown != null) StopCoroutine(currentCooldown);
                currentCooldown = StartCoroutine(Cooldown());
            }
        }
        if (!isAnimated && !isSelecting)
        {
            if (!isSelling)
            {
                if (InputActions.Instance.inventoryInput != 0)
                OpenInventory();
            }
            else
            {
                if (InputActions.Instance.pickInput != 0 && InputActions.Instance.isOpen)
                OpenInventory();
            }
        }
        if (!isCooling && InputActions.Instance.isOpen && !isSelecting)
        {
            if(InputActions.Instance.moveInventoryInput.y != 0 || InputActions.Instance.moveInventoryInput.x != 0)
            {
                HoverSlot();
            }
            if (InputActions.Instance.inventorySelectInput != 0)
            {
                if (isSelling)
                {
                    if (selectedSlot != null && selectedSlot.itemPrefab != null)
                    {
                        if (SellSlot())
                        {
                            OpenInventory();
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
        if (isSwapping)
        {
            if (swappingSlot != null)
            {
                swappingSlot.QuitHover();
            }
            isCooling = true;
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
            isCooling = true;
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
        isAnimated = true;
        bubbleCountText.text = PlayerStats.Instance.bubbles.ToString();
        Vector2 target = InputActions.Instance.isOpen ? hiddenPosition : shownPosition;
        InputActions.Instance.isOpen = !InputActions.Instance.isOpen;
        selectedSlot.Hover();
        playerController.isInteracting = InputActions.Instance.isOpen;
        if (!InputActions.Instance.isOpen)
        {
            if (selectedSlot.itemPrefab != null) selectedSlot.QuitHover();
        }
        else InputActions.Instance.inventorySelectInput = 0f;
        if (currentAnimation != null) StopCoroutine(currentAnimation);
        currentAnimation = StartCoroutine(AnimatePanel(target));
    }
    
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
        if (!InputActions.Instance.isOpen)
        {
            isSelling = false;
            if (regularDialogueBubble.isChoosing)
            regularDialogueBubble.isChoosing = false;
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
    public void Sell()
    {
        isSelling = true;
        if (InputActions.Instance.isOpen || isAnimated || isSelecting) 
        {
            Debug.Log("InputActions.Instance.isOpen="+InputActions.Instance.isOpen+",isAnimated="+isAnimated+",isSelecting="+isSelecting);
            return;
        }
        OpenInventory();
    }
    private bool SellSlot()
    {
        if (currentCooldown != null) StopCoroutine(currentCooldown);
        currentCooldown = StartCoroutine(Cooldown());
        if (!selectedSlot.itemData.isSaleable) return false;
        PlayerStats.Instance.bubbles += selectedSlot.itemData.price;
        selectedSlot.Discard();
        return true;
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
        isSelecting = false;
    }
    public void Swap()
    {
        isSwapping = true;
        selectedSlot.StartGlow();
        isCooling = true;
        swappingCol = selectingCol;
        swappingRow = selectingRow;
        if(currentCooldown != null) StopCoroutine(currentCooldown);
        currentCooldown = StartCoroutine(Cooldown());
    }
    public void Drop()
    {
        selectedSlot.Drop();
        isSelecting = false;
    }
}
