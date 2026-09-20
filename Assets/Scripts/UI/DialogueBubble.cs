using UnityEngine;

public class DialogueBubble : MonoBehaviour
{
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
}
