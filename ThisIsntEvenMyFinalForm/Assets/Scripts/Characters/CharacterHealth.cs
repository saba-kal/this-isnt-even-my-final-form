using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterHealth : MonoBehaviour
{
    public delegate void HealthLostEvent(CharacterHealth characterHealth);
    public static HealthLostEvent OnHealthLost;

    public delegate void DeathEvent(CharacterHealth characterHealth);
    public static DeathEvent OnDeath;

    private PowerLevelManager _powerLevelManager;
    private int _powerLevel = 0;
    private Health _currentHealth;

    private void Awake()
    {
        _powerLevelManager = GetComponent<PowerLevelManager>();
    }

    public void SetImmune(bool immune)
    {
        _currentHealth.SetImmune(immune);
    }

    public void SetCurrentHealth(Health health, int powerLevel)
    {
        _powerLevel = powerLevel;
        _currentHealth = health;
        _currentHealth.DestroyOnDeath = false;
        _currentHealth.SetOnDeath(OnCurrentHealthDeath);
    }

    private void OnCurrentHealthDeath()
    {
        _currentHealth.gameObject.SetActive(false);
        if (_powerLevel >= _powerLevelManager.GetMaxPowerLevel())
        {
            OnDeath?.Invoke(this);
        }
        else
        {
            OnHealthLost?.Invoke(this);
        }
    }
}
