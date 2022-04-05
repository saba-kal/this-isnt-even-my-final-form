using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public static AIController Instance { get; private set; }

    [SerializeField] private float _maxVelocity = 10f;
    [SerializeField] private float _maxForce = 0.25f;
    [SerializeField] private float _timeUntilAICanStartShooting = 10f;
    [SerializeField] private Transform _playerTarget;
    [SerializeField] private GameObject _rotationBody;

    private Rigidbody2D _rigidbody;
    private Vector2 _currentVelocity;
    private ShootingManager _shootingManager;
    private PowerLevelManager _powerLevelManager;
    private CharacterHealth _characterHealth;
    private AIMovementHelper _aiMovementHelper;
    private bool _disabled = false;
    private float _timeSinceEnabled = 0f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        _rigidbody = GetComponent<Rigidbody2D>();
        _shootingManager = GetComponent<ShootingManager>();
        _powerLevelManager = GetComponent<PowerLevelManager>();
        _characterHealth = GetComponent<CharacterHealth>();
        _aiMovementHelper = GetComponent<AIMovementHelper>();
    }

    private void FixedUpdate()
    {
        if (_disabled)
        {
            return;
        }

        ApplyVelocity();
    }

    void Update()
    {
        if (_disabled)
        {
            return;
        }

        SetVelocity();
        FaceSpriteTowardsTarget();
        if (_timeSinceEnabled >= _timeUntilAICanStartShooting)
        {
            FireBullets();
        }
        _timeSinceEnabled += Time.deltaTime;
    }

    public void SetMaxVelocity(float maxVelocity)
    {
        _maxVelocity = maxVelocity;
    }

    public void SetDistanceFromPlayer(float distance)
    {
        _aiMovementHelper.SetDistanceFromPlayer(distance);
    }

    public void SetDisabled(bool disabled)
    {
        _disabled = disabled;
        _characterHealth.SetImmune(disabled);
        if (_disabled)
        {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            _timeSinceEnabled = 0f;
        }
        else
        {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public int GetPowerLevel()
    {
        return _powerLevelManager.GetPowerLevel();
    }

    private void ApplyVelocity()
    {
        _rigidbody.velocity = _currentVelocity;
    }

    private void SetVelocity()
    {
        var desiredLocation = _aiMovementHelper.GetMoveLocation();

        if (Mathf.Abs(desiredLocation.x - transform.position.x) <= 0.1f &&
            Mathf.Abs(desiredLocation.y - transform.position.y) <= 0.1f)
        {
            desiredLocation = transform.position;
        }

        var desiredVelocity = (desiredLocation - (Vector2)transform.position).normalized * _maxVelocity;

        var steeringVelocity = desiredVelocity - _currentVelocity;
        steeringVelocity = Vector2.ClampMagnitude(steeringVelocity, _maxForce);
        steeringVelocity /= _rigidbody.mass;

        _currentVelocity += steeringVelocity;
        _currentVelocity = Vector2.ClampMagnitude(_currentVelocity, _maxVelocity);
    }

    private void FaceSpriteTowardsTarget()
    {
        var relativePosition = _playerTarget.position - transform.position;
        var angle = Mathf.Atan2(relativePosition.y, relativePosition.x) * Mathf.Rad2Deg;
        _rotationBody.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void FireBullets()
    {
        _shootingManager.FireBulletShooters(CollisionLayer.EnemyBullet, BulletShooterType.Normal | BulletShooterType.Heavy);
    }
}
