using UnityEngine;
using System.Collections.Generic;
public class Dealer : NPC
{
    [SerializeField] InventoryUI inventoryScript;
    [SerializeField] Item[] items;
    public List<string> dealDialogues;
    public override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        isDialogue = false;
        for (int i = 0; i < 10; i++)
        {
            items[i] = new Item();
        }
    }
    public override void Dialogue()
    {
        base.Dialogue();
        //call options
        //define a way to call an input
        // sell Opens inventory
        // sell something
        // ask for price of that something
        // go back


        // today tasks:
        // create dialogue system
        //      create dialogue class
        //      create text dialogue child
        //      who access these values?
        //      inventory option calls inventoryUI.OpenInventory()
        //      make adaptations to inventoryUI so it handles more ways to open inventory other than using
        //      the inventory input
        // allow options in dialogue system
        // allow options to open inventory
    }
    private void Sell()
    {

    }
    void Update()
    {

    }

}
