using UnityEngine;
using System.Collections.Generic;
public class Dealer : NPC
{
    [SerializeField] List<string> options;
    [SerializeField] InventoryUI inventoryScript;
    [SerializeField] Item[] items;
    public List<string> dealDialogues;
}
