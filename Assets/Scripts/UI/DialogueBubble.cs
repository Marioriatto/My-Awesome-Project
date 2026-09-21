using UnityEngine;

public class DialogueBubble : MonoBehaviour
{
    protected RectTransform rectTransform;
    protected bool isAnimated;
    protected Coroutine currentAnimation;
    
    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    protected virtual void PopIn()
    {
        isAnimated = true;
        if (currentAnimation != null) StopCoroutine(currentAnimation);
        currentAnimation = StartCoroutine(AnimateScale(Vector3.zero, Vector3.one));
    }
    protected virtual void PopOut()
    {
        isAnimated = true;
        if (currentAnimation != null) StopCoroutine(currentAnimation);
        currentAnimation = StartCoroutine(AnimateScale(Vector3.one, Vector3.zero));
        
    }
    void Start()
    {
        Hide();
    }
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
    protected System.Collections.IEnumerator AnimateScale(Vector3 start, Vector3 target)
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
}
