using UnityEngine;
using System.Collections;

public class BulletTrailData
{
    public GameObject ParentBullet { get; set; }
    public GameObject SpawnedInstance { get; set; }
    public bool InProcessOfBeingDisabled { get; set; }
}
