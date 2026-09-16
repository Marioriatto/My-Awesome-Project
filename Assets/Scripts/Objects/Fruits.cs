using UnityEngine;

public class Fruits : Item
{
    public override void Pick()
    {
        base.Pick();
        for (int i = 0; i < 10; i++)
        {
            if (PlayerStats.Instance.items[i] == null)
            {
                PlayerStats.Instance.items[i] = new InventoryItem(itemName);
                Debug.Log("Picked up fruit: "+ itemName+" at index: "+i);
                Consume();
                return;
            }
        }
        Debug.Log("Couldnt pick up: "+ itemName);
    }
    public override void OnConsume()
    {
        base.OnConsume();
    }
}
