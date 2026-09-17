using UnityEngine;

public class Slot : MonoBehaviour
{
    //item photo
    private int _id;
    public int id { get{return _id;} set {_id = value;}}
    private RectTransform rectTransform;
    private Vector2 normalSize;
    private float hoverScaling;
    private Item _item;
    public Item item { get {return _item;} set{_item = value;}};
    void Start()
    {
        normalSize = new Vector2(250f,250f);
        hoverScaling = 1.2f;
        rectTransform = gameObject.GetComponent<RectTransform>();
    }
    public void SetContainer(Item item)
    {
        _item = item;
        // DISPLAY IMG
    }
    public void SlotAction()
    {
        Debug.Log("selected" + id);
    }
    public void QuitHover()
    {
        rectTransform.sizeDelta = normalSize;
    }
    public void Hover()
    {
        rectTransform.sizeDelta = new Vector2(normalSize.x * hoverScaling, normalSize.y * hoverScaling);
    }
    void Update()
    {
        
    }
}
