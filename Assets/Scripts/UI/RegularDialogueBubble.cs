using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class RegularDialogueBubble : DialogueBubble
{
    [SerializeField] protected TextMeshProUGUI textMeshPro; // ready
    [SerializeField] OptionsBubble optionsBubble; // ready
    // definir forma de invocar a options bubble desde la lista de dialogos
    private PlayerInputActions inputActions; // cambiar por referencia al singleton
    private List<string> dialogues;
    private bool isReady;
    private Coroutine currentCooldown;

    protected override void Awake()
    {
        base.Awake();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        currentCooldown = null;
    }
    public void SetText(List<string> content)
    {
        dialogues = content;
        PopIn();
        Typewriter();
    }
    void Start()
    {
        isReady = false;
    }
    private System.Collections.IEnumerator Typewriter()
    {
        while (isAnimated)
        {
            yield return null;
        }
        foreach(string phrase in dialogues)
        {
            while (InputActions.Instance.buttonInput == 0 || !isReady)
            {
                yield return null;
            }
            isReady = false;
            if (currentCooldown != null) StopCoroutine(currentCooldown);
            currentCooldown = StartCoroutine(TypewriterAnimation(phrase));
        }
        PopOut();
    }
    private System.Collections.IEnumerator TypewriterAnimation(string phrase)
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
        isReady = true;
    }
}