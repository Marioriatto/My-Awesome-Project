using UnityEngine;
public class Bubbles : Item
{
    private int _amount;
    private void Start()
    {
        amount = Random.Range(100, 1000);
    }
    public override void Pick()
    {
        base.Pick();
        Consume();
    }
    
    public override void OnConsume()
    {
        base.OnConsume();
        PlayerStats.Instance.bubbles += amount;
    }
    public int amount
    {
        get { return _amount; }
        set { _amount = value; }
    }
}
