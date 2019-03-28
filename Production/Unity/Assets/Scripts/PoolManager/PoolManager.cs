using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    #region Public Variables
    public static PoolManager instance;

    public List<PoolObject> pools = new List<PoolObject>();
    #endregion

    #region Private Variables
    private Dictionary<string, Queue> createdPools = new Dictionary<string, Queue>();
    #endregion

    /*
     * Awake is called when the script instance is being loaded.
     * We use it to set a static refrence to the PoolManager.
     */
    private void Awake()
    {
        instance = this;

        // Generate objects
        GeneratePools();
    }

    /*
     * Generate pools
     * First instantiate of all GameObjects so we can re-use them.
     */
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

    /*
     * Use item from poolmanager
     */
    public void InstantiateObject(string nameTag, Vector2 pos, Quaternion rot, Transform parent = null)
    {
        // Get item of queue
        GameObject go = (GameObject)createdPools[nameTag].Dequeue();

        // Set properties
        go.SetActive(true);
        go.transform.position = pos;
        go.transform.rotation = rot;
        go.transform.SetParent(parent);

        // Call onstart
        if (go.GetComponent<PoolInterface>() != null)
        {
            go.GetComponent<PoolInterface>().OnStart();
        }

        // Put back in queue
        createdPools[nameTag].Enqueue(go);
    }
}
