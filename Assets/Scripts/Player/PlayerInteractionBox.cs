using UnityEditor.Search;
using UnityEngine;

public class PlayerInteractionBox : MonoBehaviour
{
    [SerializeField] public PlayerController parentScript;
    public GameObject interactingSubject;
    [SerializeField] RegularDialogueBubble dialogueBubble;
    void Start()
    {
        interactingSubject = null;
        if (parentScript == null) Debug.LogWarning("Interactionbox does not have access to playercontroller");
        if (dialogueBubble == null) Debug.LogWarning("Interactionbox does not have access to dialoguebubble");
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            if (InputActions.Instance.buttonInput != 0 && parentScript.isInteracting == false) 
            {
                if (interactingSubject != null) return;
                interactingSubject = other.gameObject;
                parentScript.isInteracting = true;
                NPC npc = other.gameObject.GetComponent<NPC>();
                if (npc.dialogues == null) Debug.Log("no dialogues found in npc");
                else
                {
                    parentScript.RotateTowardsTarget(npc.transform.position);
                    npc.stayStill = true;
                    npc.RotateTowards(PlayerStats.Instance.transform.position);
                    dialogueBubble.SetText(npc);
                }
            }
        }
        else if (other.CompareTag("Dealer"))
        {
            if (InputActions.Instance.buttonInput != 0 && parentScript.isInteracting == false)
            {
                if (interactingSubject != null) return;
                interactingSubject = other.gameObject;
                parentScript.isInteracting = true;
                Dealer dealer = other.gameObject.GetComponent<Dealer>();
                if (dealer.dialogues == null) Debug.Log("no dialogues found in dealer");
                else
                {
                    parentScript.RotateTowardsTarget(dealer.transform.position);
                    dealer.stayStill = true;
                    dealer.RotateTowards(PlayerStats.Instance.transform.position);
                    dialogueBubble.SetText(dealer);
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
    }
}
