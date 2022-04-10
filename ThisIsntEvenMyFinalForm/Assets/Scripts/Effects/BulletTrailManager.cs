using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletTrailManager : MonoBehaviour
{
    public static BulletTrailManager Instance { get; private set; }

    [SerializeField] private List<BulletTrailPool> _bulletTrails;
    [SerializeField] private GameObject _bulletTrailsContainer;

    private Dictionary<string, BulletTrailPool> _bulletTrailDictionary;

    void Awake()
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

    private void Start()
    {
        _bulletTrailDictionary = new Dictionary<string, BulletTrailPool>();
        foreach (var trailPool in _bulletTrails)
        {
            trailPool.SpawnedTrails = new Queue<BulletTrailData>();
            for (var i = 0; i < trailPool.PoolCapacity; i++)
            {
                var trailObject = Instantiate(trailPool.Prefab, _bulletTrailsContainer.transform);
                trailObject.SetActive(false);
                trailPool.SpawnedTrails.Enqueue(new BulletTrailData
                {
                    SpawnedInstance = trailObject,
                    ParentBullet = null
                });
            }
            _bulletTrailDictionary.Add(trailPool.Name, trailPool);
        }
    }

    private void Update()
    {
        var bulletTrailsToDisabled = new List<BulletTrailData>();

        foreach (var trailPool in _bulletTrailDictionary.Values)
        {
            foreach (var trailData in trailPool.SpawnedTrails)
            {
                if (trailData.SpawnedInstance == null || !trailData.SpawnedInstance.activeSelf)
                {
                    continue;
                }

                if (trailData.ParentBullet != null && trailData.ParentBullet.activeSelf)
                {
                    trailData.SpawnedInstance.transform.position = trailData.ParentBullet.transform.position;
                }
                else if (!trailData.InProcessOfBeingDisabled)
                {
                    bulletTrailsToDisabled.Add(trailData);
                }
            }
        }

        StartCoroutine(DisableTrails(bulletTrailsToDisabled));
    }

    public void EnableTrail(string trailName, GameObject parent)
    {
        if (_bulletTrailDictionary.TryGetValue(trailName, out var trailPool))
        {
            var trailData = trailPool.SpawnedTrails.Dequeue();
            if (trailData.SpawnedInstance.activeSelf)
            {
                trailData.SpawnedInstance.SetActive(false);
            }

            trailData.ParentBullet = parent;
            trailData.SpawnedInstance.transform.position = parent.transform.position;
            trailData.SpawnedInstance.SetActive(true);

            trailPool.SpawnedTrails.Enqueue(trailData);
        }
    }

    private IEnumerator DisableTrails(List<BulletTrailData> bulletTrails)
    {
        foreach (var trail in bulletTrails)
        {
            trail.InProcessOfBeingDisabled = true;
        }

        yield return new WaitForSeconds(5f);
        foreach (var trail in bulletTrails)
        {
            trail.InProcessOfBeingDisabled = false;
            trail.SpawnedInstance.SetActive(false);
        }
    }
}
