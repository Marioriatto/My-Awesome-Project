using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    private bool isMoving;
    public bool isInteracting;
    [SerializeField] InventoryUI inventoryScript;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Bubbles"))
        {
            if (InputActions.Instance.pickInput != 0 && !isInteracting)
            {
                Bubbles bubbles = other.gameObject.GetComponent<Bubbles>();
                bubbles.Pick();
            }
        }
        if (other.CompareTag("Items"))
        {
            if (InputActions.Instance.pickInput != 0 && !isInteracting)
            {
                Item item = other.gameObject.GetComponent<Item>();
                if (inventoryScript.Add(item)) item.Pick();
            }
        }
    }
    void Start()
    {
        if (inventoryScript == null) Debug.LogWarning("PlayerController Script does not have a reference to Inventory script");
    }
    void Update()
    {
        if (!isInteracting)
        {
            transform.position += new Vector3(InputActions.Instance.moveInput.x * 5f * Time.deltaTime, 0.0f, InputActions.Instance.moveInput.y * 5f * Time.deltaTime);

            if (InputActions.Instance.isMoving)
            {
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, Mathf.Atan2(InputActions.Instance.rotateInput.y * -1f,InputActions.Instance.rotateInput.x) * Mathf.Rad2Deg, 0));

                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    1080f * Time.deltaTime);   
            }
        }
    }
}
