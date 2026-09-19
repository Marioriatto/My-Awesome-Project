using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
public class OptionsBubble : DialogueBubble
{
    [SerializeField] List<DialogueOption> options;
    [SerializeField] InventoryUI inventoryScript;
    [SerializeField] GameObject textOptionPrefab;
    private GameObject[] textOptionsList;
    private RectTransform rectTransform;
    private int selectedIndex;
    public float spacing;

    public void SetupOptions(List<DialogueOption> options, Vector2 newPosition)
    {
        Show();
        rectTransform = GetComponent<RectTransform>();
        transform.position = new Vector2(newPosition.x + 100, newPosition.y);
        this.options = options;
        textOptionsList = new GameObject[options.Count];
        DisplayOptions();
    }
    private void DisplayOptions()
    {
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, spacing * options.Count * 2f);
        for (int i = 0; i < options.Count; i++) 
        {
            Debug.Log(options[i].text);
            textOptionsList[i] = Instantiate(textOptionPrefab, transform);
            RectTransform prefabRectTransform = textOptionsList[i].GetComponent<RectTransform>();
            prefabRectTransform.anchoredPosition = new Vector2(transform.position.x, transform.position.y + 50f - (spacing * i));
            TextMeshProUGUI prefabTextMeshPro = textOptionsList[i].GetComponent<TextMeshProUGUI>();
            prefabTextMeshPro.text = options[i].text;
        }
    }
    void Update()
    {
        // hacer una funcion que haga hover sobre los textMeshPro
        if (inventoryScript.inventorySelectInput != 0 && !inventoryScript.isCooling)
        {
            inventoryScript.isCooling = true;
            options[selectedIndex].onOptionSelected?.Invoke();
            inventoryScript.isSelecting = false;
        }
    }
    void TextHover()
    {
        TextMeshProUGUI prefabTextMeshPro = textOptionsList[selectedIndex].GetComponent<TextMeshProUGUI>();
    }
    // concluyo que el optionsBubble solo se debe de encargar de:
    /*
        aparecer y desaparecer
        retornar el indice de la opcion seleccionada al inventory
        el inventory debe tener acceso al diccionario o clase del opcion:funcion
        dar valor falso al isSelecting del inventory UI al terminar
        desde aca acceder a la referencia del selectedSlot para usar Drop() o Destroy()
    */
}
