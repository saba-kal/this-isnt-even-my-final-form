using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserBulletShooter : BulletShooter
{
    [SerializeField] private List<Laser> _lasers;

    protected override void VirtualStart()
    {
        foreach (var laser in _lasers)
        {
            laser.gameObject.SetActive(false);
        }
    }

    public override void Fire(CollisionLayer layer)
    {
        foreach (var laser in _lasers)
        {
            laser.gameObject.SetActive(true);
        }
    }
}
