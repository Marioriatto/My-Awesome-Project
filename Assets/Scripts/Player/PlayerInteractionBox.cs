using UnityEditor.Search;
using UnityEngine;

public class PlayerInteractionBox : MonoBehaviour
{
    [SerializeField] PlayerController parentScript;
    void Start()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            parentScript.NpcTrigger();
        }
        else if (other.CompareTag("House"))
        {
            parentScript.HouseTrigger();
        }
    }
    void Update()
    {
        
    }
}
