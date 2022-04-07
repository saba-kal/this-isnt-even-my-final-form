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
    [SerializeField] bool _playSoundEffectOnDamage = false;

    private int _currentHealth;
    private bool _immune = false;
    private Action _onDeath = null;

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
        if (_currentHealth <= 0)
        {
            if (DestroyOnDeath) Destroy(gameObject);
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

        if (_playSoundEffectOnDamage)
        {
            if (gameObject.layer == (int)CollisionLayer.Enemy)
            {
                SoundManager.Instance?.Play(SoundClipNames.ENEMY_DAMAGE_SFX);
            }
            else
            {
                SoundManager.Instance?.Play(SoundClipNames.DAMAGE_SFX);
            }
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
}
