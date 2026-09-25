using System.Collections.Generic;
[System.Serializable]
public class Dialogues
{
    public List<string> lines;
}
[System.Serializable]
public class NPCDialoguesList
{
    public string name;
    public List<Dialogues> dialogues;
}
[System.Serializable]
public class DialogueType
{
    public string typeName;
    public List<NPCDialoguesList> npcList;
}
[System.Serializable]
public class DialoguesData
{
    public List<DialogueType> types;    
}