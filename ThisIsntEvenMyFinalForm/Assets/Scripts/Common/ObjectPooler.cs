using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance { get; private set; }
    public int SpawnedObjectCount = 0;

    [SerializeField] private List<Pool> _objectPools;

    private Dictionary<string, Queue<GameObject>> _poolDictionary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (var pool in _objectPools)
        {
            var queue = new Queue<GameObject>();
            for (var i = 0; i < pool.Size; i++)
            {
                var obj = Instantiate(pool.Prefab);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            _poolDictionary.Add(pool.GameObjectTag, queue);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!_poolDictionary.ContainsKey(tag))
        {
            return null;
        }

        SpawnedObjectCount++;
        var obj = _poolDictionary[tag].Dequeue();
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        _poolDictionary[tag].Enqueue(obj);

        return obj;
    }
}
