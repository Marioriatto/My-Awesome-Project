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
    private RectTransform rectTransform;
    private int selectedIndex; 
    [System.NonSerialized] public float spacing = 67f;
    [System.NonSerialized] public float bubbleWidth = 300f;

    private bool isAnimated;
    private bool isCooling;
    private Coroutine currentAnimation;
    private Coroutine currentCooldown;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetupOptions(List<DialogueOption> options, Vector2 newPosition)
    {
        Show();
        this.options = options;
        
        Vector2 finalSize = new Vector2(bubbleWidth, spacing * (options.Count + 1));
        rectTransform.sizeDelta = finalSize;
        rectTransform.anchoredPosition = new Vector2(newPosition.x + bubbleWidth ,newPosition.y);

        textOptionsList = new GameObject[options.Count];
        DisplayOptions();
    }
    private void DisplayOptions()
    {
        for (int i = 0; i < options.Count; i++) 
        {
            textOptionsList[i] = Instantiate(textOptionPrefab, transform);

            RectTransform prefabRectTransform = textOptionsList[i].GetComponent<RectTransform>();
            
            prefabRectTransform.anchoredPosition = new Vector2(0f, -spacing* i);

            TextMeshProUGUI prefabTextMeshPro = textOptionsList[i].GetComponent<TextMeshProUGUI>();
            prefabTextMeshPro.text = options[i].text;
            Debug.Log(prefabTextMeshPro.text);
        }
        PopIn();
    }
    void Update()
    {
        if (!isCooling && !isAnimated && inventoryScript.isSelecting)
        {
            if (inventoryScript.moveInventoryInput.y != 0)
            {
                isCooling = true;
                if (currentCooldown != null) StopCoroutine(currentCooldown);
                currentCooldown = StartCoroutine(Cooldown());
                selectedIndex -= (int)inventoryScript.moveInventoryInput.y;
                selectedIndex = (selectedIndex < 0) ? options.Count - 1 : selectedIndex % options.Count;
                Debug.Log(options[selectedIndex].text);
                TextHover();
            }
            if (inventoryScript.inventorySelectInput != 0)
            {
                options[selectedIndex].onOptionSelected?.Invoke();
                inventoryScript.isSelecting = false;
                PopOut();
            }
        }
    }
    void PopIn()
    {
        isAnimated = true;
        if (currentAnimation != null) StopCoroutine(currentAnimation);
        currentAnimation = StartCoroutine(AnimateScale(Vector3.zero, Vector3.one));
        isCooling = true;
        if (currentCooldown != null) StopCoroutine(currentCooldown);
        currentCooldown = StartCoroutine(Cooldown());
    }
    void PopOut()
    {
        isAnimated = true;
        if (currentAnimation != null) StopCoroutine(currentAnimation);
        currentAnimation = StartCoroutine(AnimateScale(Vector3.one, Vector3.zero));
    }
    private System.Collections.IEnumerator AnimateScale(Vector3 start, Vector3 target)
    {
        float elapsed = 0f;
        while (elapsed < 0.3f)
        {
            elapsed += Time.deltaTime;
            float tiempo = elapsed / 0.3f;
            rectTransform.localScale = Vector2.Lerp(start, target, tiempo);
            yield return null;
        }
        rectTransform.localScale = target;
        isAnimated = false;

        if (target == Vector3.zero) Hide();
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
    void TextHover()
    {
        RectTransform HVrectTransform = hoverPanel.GetComponent<RectTransform>();
        HVrectTransform.anchoredPosition = new Vector2(0f, -spacing * selectedIndex);
    }
}
