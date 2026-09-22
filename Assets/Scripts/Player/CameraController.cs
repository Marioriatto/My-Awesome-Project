using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] PlayerController player;
    private bool isZoomed;
    private bool isZooming;
    private Coroutine currentZoom;
    void Start()
    {
        transform.position = new Vector3(player.transform.position.x, 2.5f, player.transform.position.z - 6f);
        isZooming = false;
        isZoomed = false;
        transform.rotation = Quaternion.Euler(new Vector3(15, 0, 0));
        if (player == null)
        {
            Debug.LogWarning("CameraController has no reference to PlayerController");
        }
    }

    void LateUpdate()
    {
        if (player != null && !isZooming)
        {
            if (isZoomed) transform.position = new Vector3(player.transform.position.x, 2f, player.transform.position.z - 4f);
            else transform.position = new Vector3(player.transform.position.x, 2.5f, player.transform.position.z - 6f);
        }
    }
    public void Zoom()
    {
        if (isZooming) return;
        if (currentZoom != null) StopCoroutine(currentZoom);
        currentZoom = StartCoroutine(ZoomIn());
    }
    public void QuitZoom()
    {
        if (isZooming) return;
        if (currentZoom != null) StopCoroutine(currentZoom);
        currentZoom = StartCoroutine(ZoomOut());
    }
    private System.Collections.IEnumerator ZoomIn()
    {
        isZooming = true;
        transform.position = new Vector3(player.transform.position.x, 2.5f, player.transform.position.z - 6f);
        Vector3 start = new Vector3(0f, 2.5f, -6f);
        Vector3 target = new Vector3(0f, 2f, -4f);
        float elapsed = 0f;
        while (elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;
            float tiempo = elapsed / 0.5f;
            Vector3 currentOffset = Vector3.Lerp(start, target, tiempo);
            transform.position = new Vector3(player.transform.position.x, currentOffset.y, player.transform.position.z + currentOffset.z);
            yield return null;
        }
        isZoomed = true;
        isZooming = false;
    }
    private System.Collections.IEnumerator ZoomOut()
    {
        isZooming = true;
        transform.position = new Vector3(player.transform.position.x, 2f, player.transform.position.z - 4f);
        Vector3 start = new Vector3(0f, 2f, -4f);
        Vector3 target = new Vector3(0f, 2.5f, -6f);
        float elapsed = 0f;
        while (elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;
            float tiempo = elapsed / 0.5f;
            Vector3 currentOffset = Vector3.Lerp(start, target, tiempo);
            transform.position = transform.position = new Vector3(player.transform.position.x, currentOffset.y, player.transform.position.z + currentOffset.z);
            yield return null;
        }
        isZoomed = false;
        
        isZooming = false;
    }
}
