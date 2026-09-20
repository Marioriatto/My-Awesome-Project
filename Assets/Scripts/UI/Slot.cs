using UnityEngine;

public class Slot : MonoBehaviour
{
    //item photo
    private int _id;
    public int id { get{return _id;} set {_id = value;}}
    private GameObject _playerController;
    public GameObject playerController {get {return _playerController;} set {_playerController = value;}}
    private RectTransform rectTransform;
    private RectTransform iconRectTransform;
    private Vector2 normalSize;
    private Vector2 iconNormalSize;
    private float hoverScaling;
    private GameObject _icon;
    public GameObject icon {get{return _icon;} set { _icon = value;}}
    public GameObject itemPrefab;
    public ItemData itemData;
    void Awake()
    {
        icon = null;
        iconRectTransform = null;
        itemData = null;
        itemPrefab = null;
    }
    void Start()
    {
        iconNormalSize = new Vector2(0f,0f);
        normalSize = new Vector2(250f,250f);
        hoverScaling = 1.2f;
        rectTransform = gameObject.GetComponent<RectTransform>();
    }
    public void SetContainer(ItemData data)
    {
        if (playerController == null) Debug.LogWarning("slot "+id+" does not have access to PlayerController");
        if (data == null) 
        {
            Debug.LogWarning("item is null");
            return;
        }
        itemData = data;
        // Prefab setup
        if (data.prefab != null) itemPrefab = data.prefab;
        else Debug.LogWarning("item " + data.itemName + " doesnt have a prefab");
        // Icon setup
        icon = Instantiate(data.icon, transform);
        iconRectTransform = icon.GetComponent<RectTransform>();
        iconNormalSize = iconRectTransform.sizeDelta;
    }
    public void Drop()
    {
        Debug.Log("Drop");
        if (itemPrefab == null) {Debug.Log("no item"); return;}
        if (playerController == null) {Debug.Log("No player"); return;}

        GameObject droppedItem = Instantiate(itemPrefab);
        Vector3 playerPos = playerController.transform.position;
        droppedItem.transform.position = new Vector3(playerPos.x, 0f, playerPos.z);
        Discard();
    }
    public void Use()
    {
        if (itemPrefab == null) {Debug.Log("no item"); return;}
        if (playerController == null) {Debug.Log("No player"); return;}
    
        //TODO
        
        Discard();
    }
    public void Give()
    {
        if (itemPrefab == null) {Debug.Log("no item"); return;}
        if (playerController == null) {Debug.Log("No player"); return;}
    
        //TODO
        
        Discard();
    }
    public void Discard()
    {
        if (itemPrefab == null) {Debug.Log("no item"); return;}
        if (playerController == null) {Debug.Log("No player"); return;}
        Destroy(icon);
        icon = null;
        iconNormalSize = new Vector2(0f,0f);
        iconRectTransform = null;
        itemPrefab = null;
    }
    public void Hover()
    {
        rectTransform.sizeDelta = new Vector2(normalSize.x * hoverScaling, normalSize.y * hoverScaling);
        if (icon != null) 
        {
            iconRectTransform.sizeDelta = new Vector2(iconNormalSize.x * hoverScaling, iconNormalSize.y * hoverScaling);
        }
    }
    public void QuitHover()
    {
        rectTransform.sizeDelta = normalSize;
        if (icon != null) 
        {
            iconRectTransform.sizeDelta = new Vector2(iconNormalSize.x, iconNormalSize.y);
        }
    }
}
