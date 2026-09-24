using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using Unity.Collections.Tests.CoreCLR.TestJobs;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName;
    public List<ItemData> items;
    public List<Dialogues> dialogues;
    protected bool isAnimating, isRotating;
    public bool isDealer, stayStill;
    protected Coroutine currentAnimation, currentRotation;
    protected virtual void Start()
    {
        isDealer = false;
        stayStill = false;
        isAnimating = false;
        isRotating = false;
        // check if is not already chosen
        NPCDialoguesList npcDialoguesList = JsonLoader.Instance.npc[Random.Range(0,JsonLoader.Instance.npc.Count)];
        npcName = npcDialoguesList.name;
        dialogues = npcDialoguesList.dialogues;
    }
    protected void Update()
    {
        if (!isAnimating && !stayStill)
        {
            switch (Random.Range(0,2))
            {
                case 0:
                    if(currentAnimation != null) StopCoroutine(currentAnimation);
                    currentAnimation = StartCoroutine(StayIdle());
                break;
                case 1:
                    if (currentAnimation != null) StopCoroutine(currentAnimation);
                    currentAnimation = StartCoroutine(Move(new Vector3(Random.Range(-7.0f,7.0f),0,Random.Range(-7.0f,7.0f))));
                break;
                default:
                break;
            }
        }
    }
    public virtual void RotateTowards(Vector3 target)
    {
        StopAllCoroutines();
        if(currentRotation != null) StopCoroutine(currentRotation);
        currentRotation = StartCoroutine(Rotate(target));
    }
    private System.Collections.IEnumerator Move(Vector3 target)
    {
        isAnimating = true;
        target += transform.position;
        if(currentRotation != null) StopCoroutine(currentRotation);
        currentRotation = StartCoroutine(Rotate(target));
        while (isRotating)
        {
            yield return null;
        }
        isAnimating = true;
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 2f * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
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
        isAnimating = true;
        float elaps = 0f;
        while (elaps < Random.Range(1.0f,5.0f))
        {
            elaps += Time.deltaTime;
            yield return null;
        }
        isAnimating = false;
    }
}
