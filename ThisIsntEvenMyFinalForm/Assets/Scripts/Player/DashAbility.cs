using UnityEngine;
using System.Collections;
using System;

public class DashAbility : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private float _speed;
    [SerializeField] private float _cooldown;
    [SerializeField] private GhostEffect _ghostEffect;

    private Camera _mainCamera;
    private Rigidbody2D _rigidbody;
    private float _timeSinceLastDash = 100f;
    private Action _onActivate;

    // Use this for initialization
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _ghostEffect.enabled = false;
        _mainCamera = Camera.main;
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
        var worldMousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var direction = (worldMousePos - transform.position).normalized;

        var timeSinceAbilityStart = 0f;
        while (timeSinceAbilityStart < _duration)
        {
            var hit = Physics2D.Raycast(transform.position, direction, 1f, LayerMask.GetMask(nameof(CollisionLayer.Obstacle)));
            if (hit.collider != null)
            {
                break;
            }

            _rigidbody.transform.Translate(direction * Time.deltaTime * _speed);
            timeSinceAbilityStart += Time.deltaTime;
            yield return null;
        }
        _ghostEffect.enabled = false;
    }
}
