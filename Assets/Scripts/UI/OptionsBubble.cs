using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
public class OptionsBubble : DialogueBubble
{
    [SerializeField] List<DialogueOption> options;
    [SerializeField] InventoryUI inventoryScript;
    [SerializeField] GameObject textOptionPrefab;
    [SerializeField] GameObject hoverPanel;
    private GameObject[] textOptionsList;
    private RectTransform HPRectTransform;
    private int selectedIndex;
    [System.NonSerialized] public float spacing = 67f;
    [System.NonSerialized] public float bubbleWidth = 300f;
    protected bool isCooling;
    protected Coroutine currentCooldown;
    protected override void Awake()
    {
        base.Awake();
        isCooling = false;
        currentCooldown = null;
        selectedIndex = 0;
    }
    public void SetupOptions(List<DialogueOption> options, Vector2 newPosition)
    {
        Show();
        selectedIndex = 0;
        this.options = options;
        Vector2 finalSize = new Vector2(bubbleWidth, spacing * (options.Count + 1));
        rectTransform.sizeDelta = finalSize;
        rectTransform.anchoredPosition = new Vector2(newPosition.x + bubbleWidth ,newPosition.y);

        HPRectTransform = hoverPanel.GetComponent<RectTransform>();
        HPRectTransform.sizeDelta = new Vector2(bubbleWidth, rectTransform.sizeDelta.y / options.Count);
        HPRectTransform.anchoredPosition = new Vector2(0f, spacing * (options.Count / 2));
        textOptionsList = new GameObject[options.Count];
        DisplayOptions();
    }
    private void DisplayOptions()
    {
        for (int i = 0; i < options.Count; i++) 
        {
            textOptionsList[i] = Instantiate(textOptionPrefab, transform);

            RectTransform prefabRectTransform = textOptionsList[i].GetComponent<RectTransform>();
            
            prefabRectTransform.anchoredPosition = new Vector2(0f, (spacing * (options.Count / 2) - spacing * i) + 20);

            TextMeshProUGUI prefabTextMeshPro = textOptionsList[i].GetComponent<TextMeshProUGUI>();
            prefabTextMeshPro.text = options[i].text;
        }
        PopIn();
    }
    void Update()
    {
        if (!isCooling && !isAnimated && (inventoryScript.isSelecting || InputActions.Instance.isRegularDialogue))
        {
            if (InputActions.Instance.moveInventoryInput.y != 0)
            {
                TextHover();
                isCooling = true;
                if (currentCooldown != null) StopCoroutine(currentCooldown);
                currentCooldown = StartCoroutine(Cooldown());
                selectedIndex -= (int)InputActions.Instance.moveInventoryInput.y;
                selectedIndex = (selectedIndex < 0) ? options.Count - 1 : selectedIndex % options.Count;
                TextHover();
            }
            if (InputActions.Instance.inventorySelectInput != 0)
            {
                options[selectedIndex].onOptionSelected?.Invoke();
                PopOut();
            }
        }
    }
    void TextHover()
    {
        Vector2 target = new Vector2(0f, spacing * (options.Count / 2) - spacing * selectedIndex);
        isAnimated = true;
        if (currentAnimation != null) StopCoroutine(currentAnimation);
        currentAnimation = StartCoroutine(AnimateHoverPanel(HPRectTransform.anchoredPosition, target));
    }
    protected override void PopIn()
    {
        base.PopIn();
        isCooling = true;
        if (currentCooldown != null) StopCoroutine(currentCooldown);
        currentCooldown = StartCoroutine(Cooldown());
    }
    protected override void PopOut()
    {
        base.PopOut();
        DiscardPrefabs();
    }
    private void DiscardPrefabs()
    {
        for (int i = 0; i < options.Count; i++)
        {
            if (textOptionsList[i] != null) Destroy(textOptionsList[i]);
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
    private System.Collections.IEnumerator AnimateHoverPanel(Vector2 start, Vector2 target)
    {
        float elapsed = 0f;
        while (elapsed < 0.1f)
        {
            elapsed += Time.deltaTime;
            float tiempo = elapsed / 0.1f;
            HPRectTransform.anchoredPosition = Vector2.Lerp(start, target, tiempo);
            yield return null;
        }
        HPRectTransform.anchoredPosition = target;
        isAnimated = false;
    }
}
