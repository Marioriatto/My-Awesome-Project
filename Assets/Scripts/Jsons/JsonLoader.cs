using UnityEngine;
using System.IO;
using System.Collections.Generic;
public class JsonLoader : MonoBehaviour
{
    public static JsonLoader Instance {get; private set;}
    public List<NPCDialoguesList> npc, dealer;
    public bool[] npcAvailability, dealerAvailability;
    void LoadDialogues()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Dialogues.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            DialoguesData data = JsonUtility.FromJson<DialoguesData>(json);
            foreach (DialogueType type in data.types)
            {
                if (string.Equals(type.typeName, "NPC")) npc = type.npcList;
                else if (string.Equals(type.typeName, "Dealer")) dealer = type.npcList;
            }
        }
        else
        {
            Debug.LogWarning("No hay json");
        }
    } 
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        LoadDialogues();
        npcAvailability = new bool[npc.Count];
        for (int i = 0; i < npc.Count; i++)
        {
            npcAvailability[i] = true;
        }
        dealerAvailability = new bool[dealer.Count];
        for (int i = 0; i < dealer.Count; i++)
        {
            dealerAvailability[i] = true;
        }
    }
}
