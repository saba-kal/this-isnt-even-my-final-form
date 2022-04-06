using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class ShootingManager : MonoBehaviour
{
    private List<BulletShooter> _bulletShooters = new List<BulletShooter>();

    public void SetBulletShooters(List<BulletShooter> bulletShooters)
    {
        _bulletShooters = bulletShooters;
    }

    public void FireBulletShooters(CollisionLayer collisionLayer, BulletShooterType type)
    {
        foreach (var bulletShooter in GetBulletShooters(type))
        {
            bulletShooter.Fire(collisionLayer);
        }
    }

    public float? GetMaxBulletShooterFireRate(BulletShooterType type)
    {
        float? maxFireRate = null;
        foreach (var bulletShooter in GetBulletShooters(type))
        {
            var fireRate = bulletShooter.GetFireRate();
            if (maxFireRate == null || fireRate > maxFireRate)
            {
                maxFireRate = fireRate;
            }
        }

        return maxFireRate;
    }

    /// <summary>
    /// Sets an on fire callback function for an arbitrary bullet shooter of a given type.
    /// </summary>
    /// <param name="onFire">The callback to set.</param>
    /// <param name="type">Type of bullet shooter.</param>
    public void SetArbitraryOnFire(Action onFire, BulletShooterType type)
    {
        var bulletShooter = GetBulletShooters(type)?.FirstOrDefault();
        bulletShooter?.SetOnFire(onFire);
    }

    private List<BulletShooter> GetBulletShooters(BulletShooterType type)
    {
        var resultBulletShooters = new List<BulletShooter>();

        foreach (var bulletShooter in _bulletShooters)
        {
            if (bulletShooter.IsType(type))
            {
                resultBulletShooters.Add(bulletShooter);
            }
        }

        return resultBulletShooters;
    }
}
