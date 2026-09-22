using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class RegularDialogueBubble : DialogueBubble
{
    [SerializeField] protected TextMeshProUGUI textMeshPro;
    [SerializeField] OptionsBubble optionsBubble;
    [SerializeField] PlayerInteractionBox interactionBox;
    [SerializeField] CameraController cameraController;
    private List<string> dialogues;
    private bool isReady;
    private Coroutine currentCooldown;
    private Coroutine currentTypewriter;

    protected override void Awake()
    {
        base.Awake();
        currentCooldown = null;
        currentTypewriter = null;
        if (interactionBox == null) Debug.Log("no interaction box reference from dialogueBubble");
    }
    public void SetText(List<string> content)
    {
        cameraController.Zoom();
        dialogues = content;
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        textMeshPro.text = "";
        PopIn();
        if (currentTypewriter != null) StopCoroutine(currentTypewriter);
        currentTypewriter = StartCoroutine(Typewriter());
    }
    protected override void Start()
    {
        base.Start();
        isReady = false;
    }
    private System.Collections.IEnumerator Typewriter()
    {
        while (isAnimated)
        {
            yield return null;
        }
        isReady = true;
        foreach(string phrase in dialogues)
        {
            while ((InputActions.Instance.buttonInput == 0) || !isReady)
            {
                yield return null;
            }
            isReady = false;
            if (currentCooldown != null) StopCoroutine(currentCooldown);
            currentCooldown = StartCoroutine(TypewriterAnimation(phrase));
        }
        while (InputActions.Instance.buttonInput == 0 || !isReady)
        {
            yield return null;
        }
        Debug.Log("acabe");
        PopOut();
    }
    public override void Hide()
    {
        interactionBox.parentScript.SetInteracting(false);
        base.Hide();
        cameraController.QuitZoom();
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
        InputActions.Instance.buttonInput = 0;
    }
}