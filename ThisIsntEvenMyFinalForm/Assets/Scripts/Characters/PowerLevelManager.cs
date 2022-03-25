using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class PowerLevelManager : MonoBehaviour
{
    public delegate void PowerUpEvent(int powerLevel);
    public static PowerUpEvent OnPowerUp;

    [SerializeField] private int _maxPowerLevel = 2;
    [SerializeField] private List<CharacterPowerLevelData> _powerLevels;

    private int _currentPowerLevel = 1;

    private void Start()
    {
        SetDataForCurrentPowerLevel();
    }

    public void PowerUp(int healthIndex)
    {
        if (_currentPowerLevel >= _maxPowerLevel)
        {
            return;
        }

        _currentPowerLevel = healthIndex + 1;
        SetDataForCurrentPowerLevel();

        OnPowerUp?.Invoke(_currentPowerLevel);
    }

    public int GetPowerLevel()
    {
        return _currentPowerLevel;
    }

    private void SetDataForCurrentPowerLevel()
    {
        var currentPowerLevel = _powerLevels.FirstOrDefault();
        foreach (var powerLevel in _powerLevels)
        {
            if (powerLevel.PowerLevel == _currentPowerLevel)
            {
                powerLevel.Sprite.SetActive(true);
                currentPowerLevel = powerLevel;
            }
            else
            {
                powerLevel.Sprite.SetActive(false);
            }
        }

        var playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.SetMaxVelocity(currentPowerLevel.CharacterSpeed);
        }

        var aiController = GetComponent<AIController>();
        if (aiController != null)
        {
            aiController.SetMaxVelocity(currentPowerLevel.CharacterSpeed);
            aiController.SetDistanceFromPlayer(currentPowerLevel.AIDesiredDistanceFromPlayer);
        }
    }
}
