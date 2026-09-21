using UnityEngine;

public class NPC : MonoBehaviour
{
    private static readonly string[] names =
    {
        "Marlon", "Charlie", "George", "Ian"
    };
    protected bool isDialogue;
    protected bool isMoving;
    public static int npcID;
    private string _npcName;
    public string npcName { get{ return _npcName;} set{_npcName = value;}}
    public virtual void Awake()
    {
        npcName = names[Random.Range(0,names.Length)];
    }
    protected virtual void Start()
    {
        isMoving = false;
    }
    public virtual void Dialogue()
    {
        isDialogue = true;
    }
    protected virtual void Movement()
    {
        /*
            define random target
                rotation and distance (polar coordinates)
            define random idle time
        */
    }
    protected virtual Vector3 SetTarget()
    {
        return new Vector3(Random.Range(-10f,10f),0,Random.Range(-10f,10f));
    }   
    void Update()
    {
        if (isDialogue)
        {
            //prob this will be done by dialogue UI
            //call select func
            //if sell or buy
        }
        if (!isMoving)
        {
            //Movement  
            Movement();    
        }  
    }
}
