using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class RegularDialogueBubble : DialogueBubble
{
    [SerializeField] protected TextMeshProUGUI textMeshPro;
    [SerializeField] protected TextMeshProUGUI nameTextMeshPro;
    [SerializeField] OptionsBubble optionsBubble;
    [SerializeField] PlayerInteractionBox interactionBox;
    [SerializeField] CameraController cameraController;
    private List<string> dialogues;
    private bool isReady, isFirstLine;
    private NPC npc;
    private Coroutine currentCooldown, currentTypewriter;

    protected override void Awake()
    {
        base.Awake();
        currentCooldown = null;
        currentTypewriter = null;
        if (interactionBox == null) Debug.Log("no interaction box reference from dialogueBubble");
    }
    public void SetText(NPC npc)
    {
        this.npc = npc;
        cameraController.Zoom();
        dialogues = npc.dialogues[Random.Range(0,npc.dialogues.Count)].lines;
        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        textMeshPro = texts[0];
        textMeshPro.text = "";
        nameTextMeshPro = texts[1];
        nameTextMeshPro.text = npc.npcName;
        PopIn();
        if (currentTypewriter != null) StopCoroutine(currentTypewriter);
        currentTypewriter = StartCoroutine(Typewriter());
    }
    protected override void Start()
    {
        base.Start();
        isFirstLine = false;
        isReady = false;
    }
    private System.Collections.IEnumerator Typewriter()
    {
        while (isAnimated)
        {
            yield return null;
        }
        isReady = true;
        isFirstLine = true;
        foreach(string phrase in dialogues)
        {
            while ((!isFirstLine && InputActions.Instance.buttonInput == 0) || !isReady)
            {
                yield return null;
            }
            isFirstLine = false;
            isReady = false;
            if (currentCooldown != null) StopCoroutine(currentCooldown);
            currentCooldown = StartCoroutine(TypewriterAnimation(phrase));
        }
        while (InputActions.Instance.buttonInput == 0 || !isReady)
        {
            yield return null;
        }
        PopOut();
        npc.stayStill = false;
    }
    public override void Hide()
    {
        interactionBox.interactingSubject = null;
        base.Hide();
        cameraController.QuitZoom();
        interactionBox.parentScript.SetInteracting(false);
    }
    private System.Collections.IEnumerator TypewriterAnimation(string phrase)
    {
        textMeshPro.text = "";
        foreach(char letter in phrase)
        {
            textMeshPro.text += letter;
            float elapsed = 0f;
            while (elapsed < 0.035f)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
        isReady = true;
        InputActions.Instance.buttonInput = 0;
    }
}