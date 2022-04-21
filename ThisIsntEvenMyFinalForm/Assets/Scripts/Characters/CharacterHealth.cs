using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;

public class CharacterHealth : MonoBehaviour
{
    public delegate void HealthLostEvent(CharacterHealth characterHealth);
    public static HealthLostEvent OnHealthLost;

    public delegate void DeathEvent(CharacterHealth characterHealth);
    public static DeathEvent OnDeath;

    [SerializeField] private Color _damageColor;
    [SerializeField] private float _damageColorChangeDuration = 1f;
    [SerializeField] private EventReference _takeDamageSoundEvent;
    [SerializeField] private List<GameObject> _headIcons;
    [SerializeField] private List<SpriteRenderer> _bodySprites;

    private PowerLevelManager _powerLevelManager;
    private int _powerLevel = 0;
    private Health _currentHealth;
    private Color _baseColor;

    private void Awake()
    {
        _baseColor = _bodySprites[0].color;
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
        _currentHealth.SetOnTakeDamage(OnTakeDamage);
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

    private void OnTakeDamage()
    {
        RuntimeManager.PlayOneShot(_takeDamageSoundEvent, transform.position);
        StopCoroutine("AnimateDamageColorChange");
        StartCoroutine("AnimateDamageColorChange");
    }

    private IEnumerator AnimateDamageColorChange()
    {
        foreach (var sprite in _bodySprites)
        {
            sprite.color = _damageColor;
        }

        var elapsedTime = 0f;
        while (elapsedTime < _damageColorChangeDuration)
        {
            elapsedTime += Time.deltaTime;
            foreach (var sprite in _bodySprites)
            {
                sprite.color = Color.Lerp(_damageColor, _baseColor, elapsedTime / _damageColorChangeDuration);
            }
            yield return null;
        }

        foreach (var sprite in _bodySprites)
        {
            sprite.color = _baseColor;
        }
    }
}
