using UnityEngine;
using System.Collections.Generic;
public class OptionsBubble : DialogueBubble
{
    [SerializeField] private List<string> options;
    [SerializeField] private InventoryUI inventoryScript;
    private int selectedIndex;

    public event System.Action<int> OnOptionSelected;

    public void SetupOptions(List<string> options)
    {
        Show();
        this.options = options;
        DisplayOptions();
    }
    private void DisplayOptions()
    {
        foreach (string option in options) 
        {
            Debug.Log(option);
            // similar to slot instantiation, instantiate an box with text foreach option
            // only this time recalculating the size of the box according to the amount of options
        }
    }
    public void ConfirmSelection()
    {
        OnOptionSelected?.Invoke(selectedIndex);
    }
}
