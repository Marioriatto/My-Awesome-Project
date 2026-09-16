using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI bubbleCount;
    private PlayerInputActions inputActions;
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] RectTransform bubbleRectTransform;
    private RectTransform rectTransform;
    private Vector2 shownPosition;
    private Vector2 hiddenPosition;
    private float inventoryInput;

    private bool isOpen;
    private bool isAnimated;
    private Coroutine currentAnimation;
    
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Inventory.performed += OnInventoryPerformed;
        inputActions.Player.Inventory.canceled += OnInventoryCanceled;
    }
    private void OnDisable()
    {
        inputActions.Player.Inventory.performed -= OnInventoryPerformed;
        inputActions.Player.Inventory.canceled -= OnInventoryCanceled;
        inputActions.Player.Disable();
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
        rectTransform = GetComponent<RectTransform>();
        bubbleCount = GetComponentInChildren<TextMeshProUGUI>();
        inputActions = new PlayerInputActions();
        isAnimated = false;
    }
    void Start()
    {
        if (playerController == null)
        {
            Debug.Log("InventoryUI reference for playerController is null");
        }
        else
        {
            playerController.isInventoryOpen = false;
        }
        hiddenPosition = new Vector2(0f, -1500f);
        shownPosition = new Vector2(0f, 0f);
        isOpen = false;
        bubbleRectTransform.anchoredPosition = new Vector2(-550f,350f);
        rectTransform.anchoredPosition = hiddenPosition;
        bubbleCount.text = playerStats.bubbles.ToString();
    }
    void Update()
    {
        if (inventoryInput != 0 && !isAnimated)
        {
            isAnimated = true;
            bubbleCount.text = playerStats.bubbles.ToString();
            playerController.isInventoryOpen = isOpen;
            isOpen = !isOpen;
            if (currentAnimation != null) StopCoroutine(currentAnimation);
            Vector2 target = isOpen ? hiddenPosition : shownPosition;
            currentAnimation = StartCoroutine(AnimatePanel(target));
        }
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
    }
}
