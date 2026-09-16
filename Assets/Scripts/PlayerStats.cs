using UnityEngine;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] string _playerName;
    [SerializeField] int _bubbles;
    [SerializeField] List<Item> items;
    
    public string playerName
    {
            get { return _playerName; }
            set { _playerName = value; }
    }
    public int bubbles
    {
        get { return _bubbles; }
        set { _bubbles = value; }
    }

    void Start()
    {
        items = new List<Item>();
    }

    void Update()
    {
        
    }
}
