using UnityEngine;
using System.Collections;

public class PowerLevelManager : MonoBehaviour
{
    [SerializeField] private int _maxPowerLevel = 2;

    private int _currentPowerLevel = 1;

    public void PowerUp()
    {
        if (_currentPowerLevel >= _maxPowerLevel)
        {
            return;
        }

        Debug.Log("Powered up!!");
        _currentPowerLevel++;
    }

    public int GetPowerLevel()
    {
        return _currentPowerLevel;
    }
}
