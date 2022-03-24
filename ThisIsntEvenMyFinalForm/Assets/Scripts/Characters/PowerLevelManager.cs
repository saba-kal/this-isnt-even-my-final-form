using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    private void SetDataForCurrentPowerLevel()
    {
        var playerController = GetComponent<PlayerController>();
        var aiController = GetComponent<AIController>();

        foreach (var powerLevel in _powerLevels)
        {
            powerLevel.Sprite.SetActive(powerLevel.PowerLevel == _currentPowerLevel);

            if (playerController != null)
            {
                playerController.SetMaxVelocity(powerLevel.CharacterSpeed);
            }

            if (aiController != null)
            {
                aiController.SetMaxVelocity(powerLevel.CharacterSpeed);
                aiController.SetDistanceFromPlayer(powerLevel.AIDesiredDistanceFromPlayer);
            }
        }
    }
}
