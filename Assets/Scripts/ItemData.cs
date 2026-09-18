using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "NewItemData", menuName = "Data/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public GameObject icon;
    public GameObject prefab;
    public List<string> options;
    public bool isSaleable;
}
