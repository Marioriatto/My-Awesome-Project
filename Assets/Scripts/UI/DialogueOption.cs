[System.Serializable]
public class DialogueOption
{
    public string text;
    [System.NonSerialized] public System.Action onOptionSelected;
}
