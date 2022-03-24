using UnityEngine;
using System.Collections;

public class PowerLevelManager : MonoBehaviour
{
    public delegate void PowerUpEvent(int powerLevel);
    public static PowerUpEvent OnPowerUp;

    [SerializeField] private int _maxPowerLevel = 2;

    private int _currentPowerLevel = 1;

    public void PowerUp()
    {
        if (_currentPowerLevel >= _maxPowerLevel)
        {
            return;
        }

        _currentPowerLevel++;
        OnPowerUp?.Invoke(_currentPowerLevel);
    }

    public int GetPowerLevel()
    {
        return _currentPowerLevel;
    }
}
