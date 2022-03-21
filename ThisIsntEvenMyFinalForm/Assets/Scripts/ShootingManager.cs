using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ShootingManager : MonoBehaviour
{
    [SerializeField] private List<ShootingProfile> _bulletShootingProfiles;

    private PowerLevelManager _powerLevelManager;

    private void Start()
    {
        _powerLevelManager = GetComponent<PowerLevelManager>();
    }

    public void FireBulletShooters(CollisionLayer collisionLayer)
    {
        var bulletShooters = _bulletShootingProfiles.FirstOrDefault(b => b.PowerLevel == _powerLevelManager.GetPowerLevel())?.BulletShooters;
        if (bulletShooters.Any())
        {
            foreach (var bulletShooter in bulletShooters)
            {
                bulletShooter.Fire(collisionLayer);
            }
        }
    }
}
