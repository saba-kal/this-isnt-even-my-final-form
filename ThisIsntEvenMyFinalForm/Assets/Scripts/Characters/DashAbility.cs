using UnityEngine;
using System.Collections;
using System;

public class DashAbility : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private float _speed;
    [SerializeField] private float _cooldown;
    [SerializeField] private GhostEffect _ghostEffect;

    private CharacterHealth _characterHealth;
    private Rigidbody2D _rigidbody;
    private float _timeSinceLastDash = 100f;
    private Action _onActivate;

    // Use this for initialization
    void Awake()
    {
        _characterHealth = GetComponent<CharacterHealth>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _ghostEffect.enabled = false;
    }

    private void Update()
    {
        _timeSinceLastDash += Time.deltaTime;
    }

    public void SetOnActivate(Action onActivate)
    {
        _onActivate = onActivate;
    }

    public float GetCooldown()
    {
        return _cooldown;
    }

    public void Dash()
    {
        if (_timeSinceLastDash < _cooldown)
        {
            return;
        }

        _ghostEffect.enabled = true;
        _timeSinceLastDash = 0f;
        StartCoroutine(MoveTowardVelocity());
        _onActivate?.Invoke();
    }

    private IEnumerator MoveTowardVelocity()
    {
        var direction = _rigidbody.velocity.normalized;
        var timeSinceAbilityStart = 0f;
        while (timeSinceAbilityStart < _duration)
        {
            _rigidbody.transform.Translate(direction * Time.deltaTime * _speed);
            timeSinceAbilityStart += Time.deltaTime;
            yield return null;
        }
        _ghostEffect.enabled = false;
    }
}
