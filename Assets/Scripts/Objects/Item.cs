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
    
    }
    public virtual void OnConsume()
    {
    
    }
    public void Consume()
    {
        OnConsume();
        Destroy(gameObject);
    }
}
