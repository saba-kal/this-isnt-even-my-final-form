using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class ShootingProfile
{
    [Range(1, 10)]
    public int PowerLevel = 1;

    public GameObject BulletShootersContainer;
}
