using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] PlayerController player;
    void Start()
    {
        transform.rotation = Quaternion.Euler(new Vector3(15, 0, 0));
        if (player == null)
        {
            Debug.LogWarning("CameraController has no reference to PlayerController");
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.transform.position.x, 2.5f, player.transform.position.z - 5f);
            
        }
    }
}
