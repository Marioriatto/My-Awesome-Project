using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] string _itemName;
    public string itemName
    {
        get { return _itemName; }
    }
    protected virtual void Pick()
    {
    
    }
    protected virtual void OnConsume()
    {
    
    }
    public void Consume()
    {
        OnConsume();
        Destroy(gameObject);
    }
}
