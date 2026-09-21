using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class RegularDialogueBubble : DialogueBubble
{
    [SerializeField] protected TextMeshProUGUI textMeshPro;
    [SerializeField] private GameObject dialoguePanel;
    private List<string> dialogues;
    private Coroutine currentCooldown;
    public void SetText(string content)
    {
        
    }
    void Update()
    {
        /*
        if (input != 0)
        {
            
        }
        */
    }
    // hacer la funcion para typewriter del textmeshpro
    // corutina con concatenacion cada 0.1 segs
    // crear una funcion que recorra la lista de cadenas de dialogo
    // esa funcion llama a la animacion typewriter
    // al terminar de animar activar la opcion de avanzar
    void Typewriter()
    {
        /*
        foreach(string phrase in dialogues)
        {
            if (currentCooldown != null) StopCoroutine(currentCooldown);
            currentCooldown = StartCoroutine(TypewriterCooldown(phrase);
            avanzar solo si presiona el boton qliao
        }
        */
        //isContinuable = true;
    }    
    private System.Collections.IEnumerator TypewriterCooldown(string phrase)
    {
        textMeshPro.text = "";
        foreach(char letter in phrase)
        {
            textMeshPro.text += letter;
            float elapsed = 0f;
            while (elapsed < 0.05f)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
        isAnimated = false;
    }
}
