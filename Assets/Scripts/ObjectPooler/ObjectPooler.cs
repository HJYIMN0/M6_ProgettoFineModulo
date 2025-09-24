using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public string tag; // Un nome per identificare la pool 
    public List<GameObject> prefab = new List<GameObject>();
    public int size; // Il numero di oggetti da creare all'inizio
}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Dizionario per tenere traccia dei prefab originali per ogni pool
    private Dictionary<string, List<GameObject>> prefabsByTag;

    void Awake()
    {
        Instance = this;
        InitializePools();
    }

    void InitializePools()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        prefabsByTag = new Dictionary<string, List<GameObject>>();

        foreach (Pool pool in pools)
        {
            if (!poolDictionary.ContainsKey(pool.tag))
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();
                prefabsByTag[pool.tag] = new List<GameObject>(pool.prefab);

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject prefabToUse = pool.prefab[Random.Range(0, pool.prefab.Count)];
                    GameObject obj = Instantiate(prefabToUse, this.transform);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                poolDictionary.Add(pool.tag, objectPool);
                Debug.Log($"Pool '{pool.tag}' inizializzato con {pool.size} oggetti");
            }
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("La Pool con il tag " + tag + " non esiste.");
            LogAvailablePools(); // Debug helper
            return null;
        }

        GameObject objectToSpawn;

        // Se ci sono oggetti disponibili nel pool
        if (poolDictionary[tag].Count > 0)
        {
            objectToSpawn = poolDictionary[tag].Dequeue();
            Debug.Log($"Spawn da pool '{tag}'. Oggetti rimasti: {poolDictionary[tag].Count}");
        }
        else
        {
            // Pool vuoto, crea un nuovo oggetto
            Debug.LogWarning($"Pool '{tag}' vuoto, creo nuovo oggetto");
            if (prefabsByTag.ContainsKey(tag) && prefabsByTag[tag].Count > 0)
            {
                GameObject prefabToUse = prefabsByTag[tag][Random.Range(0, prefabsByTag[tag].Count)];
                objectToSpawn = Instantiate(prefabToUse, this.transform);
                objectToSpawn.SetActive(false);
            }
            else
            {
                Debug.LogError($"Nessun prefab disponibile per '{tag}'");
                return null;
            }
        }

        if (objectToSpawn == null)
        {
            Debug.LogError($"Oggetto null nel pool '{tag}'");
            return null;
        }

        // Imposta posizione e rotazione prima di attivare
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    // QUESTO METODO MANCAVA NEL TUO CODICE ORIGINALE
    public void ReturnToPool(GameObject obj, string poolTag)
    {
        if (obj == null)
        {
            Debug.LogWarning("Tentativo di restituire oggetto null al pool");
            return;
        }

        if (!poolDictionary.ContainsKey(poolTag))
        {
            Debug.LogWarning($"Pool '{poolTag}' non esiste per il return");
            LogAvailablePools(); // Debug helper
            return;
        }

        // Disattiva l'oggetto e rimettilo nel pool
        obj.SetActive(false);
        obj.transform.SetParent(this.transform);
        poolDictionary[poolTag].Enqueue(obj);

        Debug.Log($"Oggetto restituito al pool '{poolTag}'. Pool ora ha {poolDictionary[poolTag].Count} oggetti");
    }

    // Helper per debug
    private void LogAvailablePools()
    {
        Debug.Log("Pool disponibili:");
        foreach (var kvp in poolDictionary)
        {
            Debug.Log($"- '{kvp.Key}' con {kvp.Value.Count} oggetti");
        }
    }

    // Metodo pubblico per controllare lo stato
    public void LogPoolStatus()
    {
        Debug.Log("=== STATUS POOL ===");
        foreach (var kvp in poolDictionary)
        {
            Debug.Log($"Pool '{kvp.Key}': {kvp.Value.Count} oggetti disponibili");
        }
    }
}