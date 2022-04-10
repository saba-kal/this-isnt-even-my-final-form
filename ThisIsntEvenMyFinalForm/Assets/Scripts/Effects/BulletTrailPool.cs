using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class BulletTrailPool
{
    public string Name;
    public GameObject Prefab;
    public int PoolCapacity;

    public Queue<BulletTrailData> SpawnedTrails { get; set; }
}
