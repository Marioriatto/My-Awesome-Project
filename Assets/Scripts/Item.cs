using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] string _itemName;
    public string itemName
    {
        get { return _itemName; }
    }
    public virtual void Pick()
    {
        Debug.Log("Picked up: " + itemName);
    }
    public virtual void OnConsume()
    {
        Debug.Log("Destroying: " + itemName);
    }
    public void Consume()
    {
        OnConsume();
        Destroy(gameObject);
    }
}
