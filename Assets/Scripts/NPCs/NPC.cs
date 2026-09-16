using UnityEngine;

public class NPC : MonoBehaviour
{
    void Start()
    {
        
    }
    protected virtual void Movement()
    {
        //rotate towards target
    }
    protected virtual Vector3 SetTarget(int x, int z)
    {
        return new Vector3(x,0,z);
    }
    void Update()
    {
        //Movement  
        Movement();   
    }
}
