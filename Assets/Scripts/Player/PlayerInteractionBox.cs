using UnityEditor.Search;
using UnityEngine;

public class PlayerInteractionBox : MonoBehaviour
{
    [SerializeField] PlayerController parentScript;
    void Start()
    {
        if (parentScript == null) Debug.LogWarning("Interactionbox does not have access to playercontroller");
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            if (InputActions.Instance.buttonInput != 0) 
            {
                parentScript.isInteracting = true;
                Debug.Log("npc TODO");
                parentScript.isInteracting = false;
            }
        }
        else if (other.CompareTag("House"))
        {
            if (InputActions.Instance.buttonInput != 0)
            {
                parentScript.isInteracting = true;
                Debug.Log("casa TODO");
                parentScript.isInteracting = false;
            }
        }
        else if (other.CompareTag("Dealer"))
        {
            if (InputActions.Instance.pickInput != 0)
            {
                //NPC dealer = other.gameObject.GetComponent<NPC>();
                //dealer.Dialogue();
            }
        }
    }
    void Update()
    {
        
    }
}
