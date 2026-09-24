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
    public Vector2 optionsPosition, shownPosition, hiddenPosition;
    private bool isReady, isFirstLine;
    public bool isChoosing;
    private NPC npc;
    private Coroutine currentCooldown, currentTypewriter;

    // si vuelvo a dar vender deja de funcionar la wea
    protected override void Awake()
    {
        base.Awake();
        shownPosition = new Vector2(0f,-270f);
        hiddenPosition = new Vector2(0f, -1500f);
        currentCooldown = null;
        currentTypewriter = null;
        if (interactionBox == null) Debug.Log("no interaction box reference from dialogueBubble");
    }
    public void Buy()
    {
        foreach(ItemData item in npc.items)
        {
            Debug.Log(item);
        }
        // creo que hay un bug con el ultimo dialogo donde me puedo seguir moviendo
        // necesito la lista o forma de escoger los items a comprars
        // afeitate la tota porque hoy te lo voa metel
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
        // probar a simplemente asignar el false de una
        interactionBox.parentScript.SetInteracting(InputActions.Instance.isOpen);
    }
    public System.Collections.IEnumerator AnimatePanel(Vector2 target)
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
        textMeshPro.text = "";
        if (target == shownPosition)
        {
            isChoosing = false;
        }
        //this line of code fixed inventory not opening
        inventoryUI.currentAnimation = null;
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