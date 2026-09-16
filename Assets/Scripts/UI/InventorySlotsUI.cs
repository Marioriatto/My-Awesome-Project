using UnityEngine;

public class InventorySlotsUI : MonoBehaviour
{
    [SerializeField] GameObject slotPrefab;
    [SerializeField] Transform gridParent;
    [SerializeField] Vector2 startPosition = new Vector2(-650f, 50f);
    [SerializeField] Vector2 spacing = new Vector2(330f, 300f);
    private int columns = 5;
    private int slotCount = 10;

    void Start()
    {
        for (int i = 1; i < slotCount; i++)
        {
            GameObject slot = Instantiate(slotPrefab, gridParent);
            int row = i / columns;
            int col = i % columns;

            RectTransform rectTransform = slot.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(
                startPosition.x + col * spacing.x,
                startPosition.y - row * spacing.y
            );
        }
    }

    void Update()
    {
        
    }
}
