using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterHealth : MonoBehaviour
{
    public delegate void HealthLostEvent(CharacterHealth characterHealth);
    public static HealthLostEvent OnHealthLost;

    public delegate void DeathEvent(CharacterHealth characterHealth);
    public static DeathEvent OnDeath;

    [SerializeField] private List<GameObject> _headIcons;

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
        UpdateHeadIcons();
    }

    private void OnCurrentHealthDeath()
    {
        _currentHealth.MakeInactive();
        if (_powerLevel >= _powerLevelManager.GetMaxPowerLevel())
        {
            OnDeath?.Invoke(this);
        }
        else
        {
            OnHealthLost?.Invoke(this);
        }
    }

    private void UpdateHeadIcons()
    {
        foreach (var icon in _headIcons)
        {
            icon.SetActive(false);
        }

        _headIcons[Mathf.Clamp(_powerLevel - 1, 0, _headIcons.Count)].SetActive(true);
    }
}
