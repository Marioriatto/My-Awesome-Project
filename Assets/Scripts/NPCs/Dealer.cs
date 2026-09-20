using UnityEngine;
using System.Collections.Generic;
public class Dealer : NPC
{
    [SerializeField] List<string> options;
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
    }
    public override void Dialogue()
    {
        base.Dialogue();
        inventoryScript.isSelling = true;
        inventoryScript.OpenInventory();
        // create dialogue system
        //      create dialogue class
        //      create text dialogue child
    }

}
