using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class RegularDialogueBubble : DialogueBubble
{
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] protected TextMeshProUGUI textMeshPro;
    [SerializeField] protected TextMeshProUGUI nameTextMeshPro;
    [SerializeField] OptionsBubble optionsBubble;
    [SerializeField] PlayerInteractionBox interactionBox;
    [SerializeField] CameraController cameraController;
    private List<string> dialogues;
    private List<DialogueOption> options;
    private Vector2 optionsPosition;
    private bool isReady, isFirstLine;
    public bool isChoosing;
    private NPC npc;
    private Coroutine currentCooldown, currentTypewriter;

    protected override void Awake()
    {
        base.Awake();
        currentCooldown = null;
        currentTypewriter = null;
        if (interactionBox == null) Debug.Log("no interaction box reference from dialogueBubble");
    }
    public void Buy()
    {
        // TODO
        // find the best way to buy items
        // validate playerStats.Instance has enough money
        Debug.Log(PlayerStats.Instance.bubbles);
        Debug.Log(npc.items);
    }
    public void Back() {isChoosing = false;}
    public void SetText(NPC npc)
    {
        if (npc.isDealer)
        {
            options = new List<DialogueOption>();
            foreach (string optionText in npc.dialogues[npc.dialogues.Count-1].lines)
            {
                System.Action action = optionText switch
                {
                    "Sell" => inventoryUI.Sell,
                    "Buy" => Buy,
                    "Back" => Back,
                    _ => null
                };
                options.Add(new DialogueOption { text = optionText, onOptionSelected = action});
            }
            dialogues = npc.dialogues[Random.Range(0,npc.dialogues.Count-2)].lines;
        }
        else
        {
            dialogues = npc.dialogues[Random.Range(0,npc.dialogues.Count)].lines;
        }

        this.npc = npc;
        cameraController.Zoom();

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
        isChoosing = false;
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
        isChoosing = true;
        if (npc.isDealer)
        {
            while (InputActions.Instance.buttonInput == 0 || !isReady)
            {
                yield return null;
            }
            InputActions.Instance.isRegularDialogue = true;
            optionsBubble.SetupOptions(options, optionsPosition);
            // in some sort of way hide the dialogue menu
            while(isChoosing)
            {
                yield return null;
            }
            string phrase = npc.dialogues[npc.dialogues.Count-2].lines[0];
            isReady = false;
            if (currentCooldown != null) StopCoroutine(currentCooldown);
            currentCooldown = StartCoroutine(TypewriterAnimation(phrase));
        }
        while (InputActions.Instance.buttonInput == 0 || !isReady)
        {
            yield return null;
        }
        PopOut();
        InputActions.Instance.isRegularDialogue = false;
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