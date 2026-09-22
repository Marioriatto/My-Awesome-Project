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
    protected bool isAnimating;
    protected bool isRotating;
    protected Coroutine currentAnimation;
    protected Coroutine currentRotation;
    public virtual void Awake()
    {
        npcName = names[Random.Range(0,names.Length)];
    }
    protected virtual void Start()
    {
        isAnimating = false;
        isRotating = false;
    }
    protected void Update()
    {
        /*
        if (!isAnimating)
        {
            switch (Random.Range(0,3))
            {
                case 0:
                    if(currentAnimation != null) StopCoroutine(currentAnimation);
                    currentAnimation = StartCoroutine(StayIdle());
                break;
                case 1:
                    if (currentAnimation != null) StopCoroutine(currentAnimation);
                    currentAnimation = StartCoroutine(MoveTowards(new Vector3(Random.Range(0.0f,1.0f),0,Random.Range(0.0f,1.0f))));
                break;
                default:
                break;
            }
        }*/
    }
    public virtual void RotateTowards(Vector3 target)
    {
        if(currentRotation != null) StopCoroutine(currentRotation);
        currentRotation = StartCoroutine(Rotate(target));
    }
    private System.Collections.IEnumerator MoveTowards(Vector3 target)
    {
        isAnimating = true;
        float elapsed = 0f;
        float timeLimit =  Random.Range(-1.0f,1.0f);

        if(currentRotation != null) StopCoroutine(currentRotation);
        currentRotation = StartCoroutine(Rotate(target));
        while (isRotating)
        {
            yield return null;
        }
        
        while (elapsed < timeLimit)
        {
            elapsed += Time.deltaTime;
            float tiempo = elapsed / timeLimit;
            transform.position = Vector3.Lerp(transform.position, transform.position + target, tiempo);
            yield return null;
        }
        isAnimating = false;
    }
    private System.Collections.IEnumerator Rotate(Vector3 target)
    {
        isAnimating = true;
        isRotating = true;
        float deltay = target.z - transform.position.z;
        float deltax = target.x - transform.position.x;
        Quaternion targetRotation = Quaternion.Euler(
            new Vector3(0,
            Mathf.Atan2(deltay * -1, deltax) * Mathf.Rad2Deg,
            0));
        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                360f * Time.deltaTime);  
            yield return null;
        }
        isRotating = false;
        isAnimating = false;
    }
    private System.Collections.IEnumerator StayIdle()
    {
        isAnimating = false;
        float elaps = 0f;
        while (elaps < Random.Range(1.0f,5.0f))
        {
            elaps += Time.deltaTime;
            yield return null;
        }
        isAnimating = true;
    }
}
