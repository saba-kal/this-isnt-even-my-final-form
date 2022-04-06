using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class PowerLevelManager : MonoBehaviour
{
    public delegate void PowerUpEvent(int powerLevel);
    public static PowerUpEvent OnPowerUp;

    [SerializeField] private int _currentPowerLevel = 0;
    [SerializeField] private List<CharacterPowerLevelData> _powerLevels;

    private int _maxPowerLevel = 2;
    private CharacterHealth _characterHealth;
    private ShootingManager _shootingManager;
    private CircleCollider2D _collider;

    private void Awake()
    {
        _characterHealth = GetComponent<CharacterHealth>();
        _shootingManager = GetComponent<ShootingManager>();
        _collider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        _maxPowerLevel = _powerLevels.Max(p => p.PowerLevel);
        SetDataForCurrentPowerLevel();
    }

    public void PowerUp()
    {
        if (_currentPowerLevel >= _maxPowerLevel)
        {
            return;
        }

        _currentPowerLevel++;
        SetDataForCurrentPowerLevel();

        OnPowerUp?.Invoke(_currentPowerLevel);
    }

    public int GetPowerLevel()
    {
        return _currentPowerLevel;
    }

    public int GetMaxPowerLevel()
    {
        return _maxPowerLevel;
    }

    public bool ReachedMaxPowerLevel()
    {
        return _currentPowerLevel >= _maxPowerLevel;
    }

    private void SetDataForCurrentPowerLevel()
    {
        var currentPowerLevelData = GetCurrentPowerLevelData();
        UpdateSprite();
        UpdatePlayerController(currentPowerLevelData);
        UpdateAIController(currentPowerLevelData);
        UpdateHealth(currentPowerLevelData);
        UpdateBulletShooters(currentPowerLevelData);
        UpdateColliderRadius(currentPowerLevelData);
    }

    private CharacterPowerLevelData GetCurrentPowerLevelData()
    {
        return _powerLevels.FirstOrDefault(p => p.PowerLevel == _currentPowerLevel);
    }

    private void UpdateSprite()
    {
        //Two loops are needed because multiple power levels may be sharing the same sprite.
        foreach (var powerLevelData in _powerLevels)
        {
            powerLevelData.Sprite.SetActive(false);
        }

        foreach (var powerLevelData in _powerLevels)
        {
            if (powerLevelData.PowerLevel == _currentPowerLevel)
            {
                powerLevelData.Sprite.SetActive(true);
                break;
            }
        }
    }

    private void UpdatePlayerController(CharacterPowerLevelData currentPowerLevelData)
    {
        var playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.SetMaxVelocity(currentPowerLevelData.CharacterSpeed);
        }
    }

    private void UpdateAIController(CharacterPowerLevelData currentPowerLevelData)
    {
        var aiController = GetComponent<AIController>();
        if (aiController != null)
        {
            aiController.SetMaxVelocity(currentPowerLevelData.CharacterSpeed);
            aiController.SetDistanceFromPlayer(currentPowerLevelData.AIDesiredDistanceFromPlayer);
        }
    }

    private void UpdateHealth(CharacterPowerLevelData currentPowerLevelData)
    {
        foreach (var powerLevelData in _powerLevels)
        {
            powerLevelData.HealthObject.gameObject.SetActive(powerLevelData.PowerLevel == _currentPowerLevel);
        }

        currentPowerLevelData.HealthObject.MaxHealth = currentPowerLevelData.HealthAmount;
        currentPowerLevelData.HealthObject.Heal();

        _characterHealth.SetCurrentHealth(currentPowerLevelData.HealthObject, _currentPowerLevel);
    }

    private void UpdateBulletShooters(CharacterPowerLevelData currentPowerLevelData)
    {
        if (currentPowerLevelData.BulletShooterContainer == null)
        {
            return;
        }

        _shootingManager.SetBulletShooters(
            currentPowerLevelData.BulletShooterContainer
                .GetComponentsInChildren<BulletShooter>().ToList());
    }

    private void UpdateColliderRadius(CharacterPowerLevelData currentPowerLevelData)
    {
        _collider.radius = currentPowerLevelData.ColliderRadius;
    }
}
