using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public bool DestroyOnDeath = true;

    [SerializeField] public int MaxHealth;
    [SerializeField] private HealthBar _healthBar;

    private int _currentHealth;
    private bool _immune = false;
    private Action _onDeath = null;

    void Start()
    {
        Heal();
    }

    public void TakeDamage(int damage)
    {
        if (_immune)
        {
            return;
        }

        _currentHealth -= damage;
        _healthBar?.SetHealth(_currentHealth, MaxHealth);
        if (_currentHealth <= 0)
        {
            if (DestroyOnDeath) Destroy(gameObject);
            _onDeath?.Invoke();
        }
    }

    public void SetImmune(bool immune)
    {
        _immune = immune;
    }

    public void SetOnDeath(Action onDeath)
    {
        _onDeath = onDeath;
    }

    public void Heal()
    {
        _currentHealth = MaxHealth;
        _healthBar?.SetHealth(_currentHealth, MaxHealth);
    }
}
