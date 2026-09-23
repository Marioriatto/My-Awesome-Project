using UnityEngine;
using System.Collections.Generic;
public class Dealer : NPC
{
    [SerializeField] List<string> options;
    public List<string> dealDialogues;
    protected override void Start()
    {
        isDealer = true;
        stayStill = false;
        isAnimating = false;
        isRotating = false;
        NPCDialoguesList npcDialoguesList = JsonLoader.Instance.dealer[Random.Range(0,JsonLoader.Instance.dealer.Count)];
        npcName = npcDialoguesList.name;
        dialogues = npcDialoguesList.dialogues;
        items = new List<ItemData>();
        for (int i = 0; i < Random.Range(2,10); i++)
        {
            items.Add(Data.Instance.data[Random.Range(0, Data.Instance.data.Count)]);
        }
    }
}
