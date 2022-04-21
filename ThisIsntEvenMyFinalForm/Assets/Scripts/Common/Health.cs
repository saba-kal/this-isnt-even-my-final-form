using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public bool DestroyOnDeath = true;

    [SerializeField] public int MaxHealth;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] float _invulnerabilityDuration = 0f;
    [SerializeField] bool _triggerChromaticAberrationOnDamage = false;
    [SerializeField] bool _triggerCameraShakeOnDamage = false;

    private int _currentHealth;
    private bool _immune = false;
    private Action _onDeath = null;
    private Action _onTakeDamage = null;

    void Start()
    {
        Heal();
    }

    public bool TakeDamage(int damage)
    {
        if (_immune)
        {
            return false;
        }

        var dead = false;

        _currentHealth -= damage;
        _healthBar?.SetHealth(_currentHealth, MaxHealth);
        _onTakeDamage?.Invoke();
        if (_currentHealth <= 0)
        {
            if (DestroyOnDeath) DestroySelf();
            _onDeath?.Invoke();
            dead = true;
        }
        else if (_invulnerabilityDuration > 0.01f)
        {
            StartCoroutine(MakeInvulnerable());
        }

        if (_triggerChromaticAberrationOnDamage)
        {
            ChromaticAberrationEffect.Instance?.Trigger();
        }

        if (_triggerCameraShakeOnDamage)
        {
            CinemachineShake.Instance?.Shake();
        }

        return dead;
    }

    public void SetImmune(bool immune)
    {
        _immune = immune;
    }

    public void SetOnDeath(Action onDeath)
    {
        _onDeath = onDeath;
    }

    public void SetOnTakeDamage(Action onDamage)
    {
        _onTakeDamage = onDamage;
    }

    public void Heal()
    {
        _currentHealth = MaxHealth;
        _healthBar?.SetHealth(_currentHealth, MaxHealth);
    }

    public void MakeInactive()
    {
        _healthBar?.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private IEnumerator MakeInvulnerable()
    {
        var originalCollisionLayer = gameObject.layer;
        transform.parent.gameObject.layer = (int)CollisionLayer.NoBulletCollision;
        _immune = true;
        yield return new WaitForSeconds(_invulnerabilityDuration);
        transform.parent.gameObject.layer = originalCollisionLayer;
        _immune = false;
    }

    private void DestroySelf()
    {
        var bulletComponent = GetComponent<BaseBullet>();
        if (bulletComponent != null)
        {
            bulletComponent.DisableBullet();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
