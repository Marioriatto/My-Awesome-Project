using UnityEditor.Search;
using UnityEngine;

public class PlayerInteractionBox : MonoBehaviour
{
    [SerializeField] public PlayerController parentScript;
    [SerializeField] RegularDialogueBubble dialogueBubble;
    void Start()
    {
        if (parentScript == null) Debug.LogWarning("Interactionbox does not have access to playercontroller");
        if (dialogueBubble == null) Debug.LogWarning("Interactionbox does not have access to dialoguebubble");
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            if (InputActions.Instance.buttonInput != 0 && parentScript.isInteracting == false) 
            {
                parentScript.isInteracting = true;
                NPC npc = other.gameObject.GetComponent<NPC>();
                if (npc.dialogues == null) Debug.Log("no dialogues found in npc");
                else 
                {
                    npc.RotateTowardsPlayer();
                    dialogueBubble.SetText(npc.dialogues);
                }
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
}
