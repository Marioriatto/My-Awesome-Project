using UnityEngine;
using System.Collections.Generic;
public class Dealer : NPC
{
    [SerializeField] List<string> options;
    public List<string> dealDialogues;
    protected override void Start()
    {
        base.Start();
        isDealer = true;
        NPCDialoguesList npcDialoguesList = JsonLoader.Instance.dealer[Random.Range(0,JsonLoader.Instance.dealer.Count)];
        npcName = npcDialoguesList.name;
        dialogues = npcDialoguesList.dialogues;
        items = new List<ItemData>();
        for (int i = 0; i < Random.Range(2,10); i++)
        {
            items.Add(Data.Instance.data[Random.Range(0, Data.Instance.data.Count)]);
        }
    }
    protected override void SetNPC()
    {
        for (int i = 0; i < JsonLoader.Instance.dealer.Count; i++)
        {
            if (JsonLoader.Instance.dealerAvailability[i])
                continue;
            else
            {
                JsonLoader.Instance.dealerAvailability[i] = false;
                NPCDialoguesList npcDialoguesList = JsonLoader.Instance.dealer[i];
                npcName = npcDialoguesList.name;
                dialogues = npcDialoguesList.dialogues;
                return;
            }
        }
        dialogues = null;
    }
}
