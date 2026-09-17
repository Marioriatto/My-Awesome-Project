using UnityEngine;

public class NPC : MonoBehaviour
{
    private static readonly string[] names =
    {
        "Marlon", "Charlie", "George", "Ian"
    };
    protected bool isDialogue;
    public static int npcID;
    private string _npcName;
    public string npcName { get{ return _npcName;} set{_npcName = value;}}
    public virtual void Awake()
    {
        npcName = names[Random.Range(0,names.Length)];
    }
    public virtual void Dialogue()
    {
        isDialogue = true;
        //TODODIALOGUE AKA CALL DIALOGUE UI
    }
    protected virtual void Movement()
    {
        //rotate towards target
    }
    protected virtual Vector3 SetTarget(int x, int z)
    {
        return new Vector3(x,0,z);
    }
    void Start()
    {
        
    }
    void Update()
    {
        if (isDialogue)
        {
            //prob this will be done by dialogue UI
            //call select func
            //if sell or buy
        }
        //Movement  
        Movement();   
    }
}
