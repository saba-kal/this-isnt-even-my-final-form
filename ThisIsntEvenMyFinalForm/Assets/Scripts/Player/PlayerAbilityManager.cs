using UnityEngine;
using System.Collections;
using System;

public class PlayerAbilityManager : MonoBehaviour
{
    private PowerLevelManager _powerLevelManager;
    private ShootingManager _shootingManager;

    void Awake()
    {
        _powerLevelManager = GetComponent<PowerLevelManager>();
        _shootingManager = GetComponent<ShootingManager>();
    }

    public void SetOnAbilityActivated(Action onAbilityActivated, PlayerAbilityType type)
    {
        switch (type)
        {
            case PlayerAbilityType.NormalShot:
                _shootingManager.SetArbitraryOnFire(onAbilityActivated, BulletShooterType.Normal);
                break;
            case PlayerAbilityType.HeavyShot:
                _shootingManager.SetArbitraryOnFire(onAbilityActivated, BulletShooterType.Heavy);
                break;
            case PlayerAbilityType.Dash:
            case PlayerAbilityType.Parry:
            default:
                break;
        }
    }

    public bool PlayerAbilityIsUnlocked(PlayerAbilityType type)
    {
        var currentPowerLevel = _powerLevelManager.GetPowerLevel();
        switch (type)
        {
            case PlayerAbilityType.NormalShot:
                return currentPowerLevel >= 1;
            case PlayerAbilityType.HeavyShot:
                return currentPowerLevel >= 2;
            case PlayerAbilityType.Dash:
                return currentPowerLevel >= 3;
            case PlayerAbilityType.Parry:
                return currentPowerLevel >= 4;
            default:
                return false;
        }
    }

    public float? GetAbilityCooldown(PlayerAbilityType type)
    {
        switch (type)
        {
            case PlayerAbilityType.NormalShot:
                return _shootingManager.GetMaxBulletShooterFireRate(BulletShooterType.Normal);
            case PlayerAbilityType.HeavyShot:
                return _shootingManager.GetMaxBulletShooterFireRate(BulletShooterType.Heavy);
            case PlayerAbilityType.Dash:
                return null;
            case PlayerAbilityType.Parry:
                return null;
            default:
                return null;
        }
    }
}
