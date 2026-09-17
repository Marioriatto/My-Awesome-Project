using UnityEngine;

public class Dealer : NPC
{
    [SerializeField] InventoryUI inventoryScript;
    public Item[] items;
    public override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        isDialogue = false;
    }
    public override void Dialogue()
    {
        base.Dialogue();
        //call options
    }
    void Update()
    {

    }

}
