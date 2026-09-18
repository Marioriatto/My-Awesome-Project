using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Data/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public GameObject icon;
    public GameObject prefab;
}
