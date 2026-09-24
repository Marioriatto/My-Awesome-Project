using System.Collections.Generic;
using UnityEngine;

public class ExitDialogueBubble : RegularDialogueBubble
{
    // write a way to save data
    private bool wasCalled;
    protected override void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        shownPosition = new Vector2(0f,-270f);
        hiddenPosition = new Vector2(0f, -1500f);
        wasCalled = false;
        currentCooldown = null;
        currentTypewriter = null;
    }
    private void Exit()
    {
       PopOut();
       #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    public override void Back()
    {
        
    }
    public void Call()
    {
        if (wasCalled) return;
        wasCalled = true;
        PopIn();
        options = new List<DialogueOption>();
        options.Add(new DialogueOption{ text = "Save and quit.", onOptionSelected = Exit});
        options.Add(new DialogueOption{ text = "Keep playing!", onOptionSelected = Back});
        if (currentTypewriter != null) StopCoroutine(currentTypewriter);
        currentTypewriter = StartCoroutine(Typewriter());
    }
    private System.Collections.IEnumerator Typewriter()
    {
        while (isAnimated)
        {
            yield return null;
        }
        isReady = false;
        if (currentAnimation != null) StopCoroutine(currentAnimation);
        currentAnimation = StartCoroutine(TypewriterAnimation("Would you like to quit now?"));
        while (InputActions.Instance.buttonInput == 0 || !isReady)
        {
            yield return null;
        }
        optionsBubble.SetupOptions(options, optionsPosition);
        isChoosing = true;
        while(isChoosing)
        {
            yield return null;
        }
        PopOut();
    }
    protected override void PopOut()
    {
        base.PopOut();
        wasCalled = false;
    }
}
