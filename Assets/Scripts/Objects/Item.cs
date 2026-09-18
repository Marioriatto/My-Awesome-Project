using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] ItemData _data;
    public ItemData data {get{return _data;} set {_data = value;}}
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
