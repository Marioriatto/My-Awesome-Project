using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using Unity.Collections.Tests.CoreCLR.TestJobs;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private static readonly string[] names =
    {
        "Marlon", "Charlie", "Jorge", "Jefry"
    };
    public static int npcID;
    private string _npcName;
    public string npcName { get{ return _npcName;} set{_npcName = value;}}
    public List<string> dialogues;
    private Vector2 target;
    protected bool isMoving;
    protected bool isRotating;
    protected Coroutine currentAnimation;
    public virtual void Awake()
    {
        npcName = names[Random.Range(0,names.Length)];
    }
    protected virtual void Start()
    {
        isMoving = false;
    }
    protected virtual void Movement()
    {
        if(currentAnimation != null) StopCoroutine(currentAnimation);
        currentAnimation = StartCoroutine(StayIdle());
    }
    public virtual void RotateTowardsPlayer()
    {
        if(currentAnimation != null) StopCoroutine(currentAnimation);
        currentAnimation = StartCoroutine(Rotate());
    }
    private System.Collections.IEnumerator Rotate()
    {
        float deltay = PlayerStats.Instance.transform.position.z - transform.position.z;
        float deltax = PlayerStats.Instance.transform.position.x - transform.position.x;
        Quaternion targetRotation = Quaternion.Euler(
            new Vector3(0,
            Mathf.Atan2(deltay, deltax) * Mathf.Rad2Deg,
            0));
        float elaps = 0f;
        // arreglar rotation
        while (transform.rotation != targetRotation)
        {
            elaps += Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                360f * Time.deltaTime);  
            yield return null;
        }
        isRotating = false;
    }    
    private System.Collections.IEnumerator StayIdle()
    {
        isMoving = false;
        float elaps = 0f;
        while (elaps < Random.Range(1,5))
        {
            elaps += Time.deltaTime;
            yield return null;
        }
        isMoving = true;
    }
}
