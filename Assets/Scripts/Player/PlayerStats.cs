using UnityEngine;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    //Singleton
    public static PlayerStats Instance { get; private set;}
    [SerializeField] string _playerName;
    [SerializeField] int _bubbles;
    public string playerName { get { return _playerName; } set { _playerName = value; }}
    public int bubbles { get { return _bubbles; } set { _bubbles = value; }}
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
