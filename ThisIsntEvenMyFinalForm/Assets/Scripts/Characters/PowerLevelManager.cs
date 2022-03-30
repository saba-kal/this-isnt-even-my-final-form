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
    [SerializeField] private int _currentPowerLevel = 0;
    [SerializeField] private List<CharacterPowerLevelData> _powerLevels;

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

        //DialogueManager.Instance.StartConversation()
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
        foreach (var powerLevel in _powerLevels)
        {
            powerLevel.Sprite.SetActive(false);
        }

        var currentPowerLevelData = _powerLevels.FirstOrDefault();
        foreach (var powerLevelData in _powerLevels)
        {
            if (powerLevelData.PowerLevel == _currentPowerLevel)
            {
                powerLevelData.Sprite.SetActive(true);
                currentPowerLevelData = powerLevelData;
                break;
            }
        }

        var playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.SetMaxVelocity(currentPowerLevelData.CharacterSpeed);
        }

        var aiController = GetComponent<AIController>();
        if (aiController != null)
        {
            aiController.SetMaxVelocity(currentPowerLevelData.CharacterSpeed);
            aiController.SetDistanceFromPlayer(currentPowerLevelData.AIDesiredDistanceFromPlayer);
        }
    }
}
