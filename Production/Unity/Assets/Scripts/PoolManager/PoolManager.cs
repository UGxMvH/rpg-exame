using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    private Dictionary<string, Queue> createdPools = new Dictionary<string, Queue>();

    public List<PoolObject> pools = new List<PoolObject>();

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;

        // Generate objects
        GeneratePools();
    }

    private void GeneratePools()
    {
        foreach(PoolObject pool in pools)
        {
            // Create pool
            createdPools.Add(pool.tag, new Queue());

            // Create objects
            for(int i = 0; i < pool.amount; i++)
            {
                GameObject go = Instantiate(pool.prefab, Vector3.zero, Quaternion.identity);
                createdPools[pool.tag].Enqueue(go);
                go.SetActive(false);
            }
        }
    }

    public void InstantiateObject(string nameTag, Vector2 pos, Quaternion rot)
    {
        // Get item of queue
        GameObject go = (GameObject)createdPools[nameTag].Dequeue();

        // Set properties
        go.SetActive(true);
        go.transform.position = pos;
        go.transform.rotation = rot;

        // Call onstart
        if (go.GetComponent<PoolInterface>() != null)
        {
            go.GetComponent<PoolInterface>().OnStart();
        }

        // Put back in queue
        createdPools[nameTag].Enqueue(go);
    }
}
