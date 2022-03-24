using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public int MaxHealth;
    [SerializeField] private HealthBar _healthBar;

    private int _currentHealth;
    private bool _immune = false;

    void Start()
    {
        _currentHealth = MaxHealth;
        _healthBar?.SetHealth(_currentHealth, MaxHealth);
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
            Destroy(gameObject);
        }
    }

    public void SetImmune(bool immune)
    {
        _immune = immune;
    }
}
