using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{    [SerializeField] GameObject[] fruits;
    [SerializeField] GameObject bubbles;
    [SerializeField] GameObject NPC;
    [SerializeField] GameObject Dealer;
    [SerializeField] GameObject Ground;
    private BitArray map;
    void SetCell(int x, int z, bool value) => map[(z * 150) + x] = value;
    bool GetCell(int x, int z) => map[(z * 150) + x];
    void StartMatrix()
    {
        map = new BitArray(150 * 150);
        for (int row = 0; row < 150; row++)
        {
            for (int col = 0; col < 150; col++)
            {
                SetCell(row,col,false);
            }
        }
    }
    bool CheckMatrix(int x, int z)
    {
        return GetCell(x + 75, z + 75);
    }
    void SpawnItems()
    {
        for (int i = 0; i < Random.Range(5,20); i++)
        {
            GameObject instance;
            //solo spawnean items
            int desicion = Random.Range(0,1), x = Random.Range(-25,25), z = Random.Range(-25,25);
            if (desicion == 0) instance = Instantiate(fruits[Random.Range(0, fruits.Length - 1)]);
            else instance = Instantiate(bubbles);
            int deltax = 1;
            // deltay = 1;
            while (CheckMatrix(x,z))
            {
                if (CheckMatrix(x + deltax, z)) x += deltax;
                else if (CheckMatrix(x - deltax, z)) x -= deltax;
                else deltax += 1;
                if (deltax == 149) break;
            }
            instance.transform.position = new Vector3(x, 0f, z);
            SetCell(x+75,z+75,true);
        }
    }
    void SpawnNPCs()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject npc;
            int desicion = Random.Range(0,2), x = Random.Range(-25,25), z = Random.Range(-25,25);
            if (desicion == 0) npc = Instantiate(NPC);
            else {npc = Instantiate(Dealer);}
            int deltax = 1;
            while (CheckMatrix(x,z))
            {
                if (CheckMatrix(x + deltax, z)) x += deltax;
                else if (CheckMatrix(x - deltax, z)) x -= deltax;
                else deltax += 1;
                if (deltax == 149) break;
            }
            npc.transform.position = new Vector3(x, 1f, z);
            SetCell(x+75,z+75,true);
        }
    }
    void SpawnHouses()
    {

    }
    void Awake()
    {
        //Spawn Ground
        if (Ground == null) Debug.LogWarning("Spawner does not have a ground prefab");
        Instantiate(Ground);
        StartMatrix();
    }
    void Start()
    {
        SpawnItems();
        SpawnNPCs();
        //SpawnHouses();
    }
    void Update()
    {
        
    }
}
