using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public static AIController Instance { get; private set; }

    [SerializeField] private float _maxVelocity = 10f;
    [SerializeField] private float _maxForce = 0.25f;
    [SerializeField] private float _desiredDistanceFromPlayer = 1f;
    [SerializeField] private float _timeUntilAICanStartShooting = 10f;
    [SerializeField] private Transform _playerTarget;
    [SerializeField] private GameObject _rotationBody;

    private Rigidbody2D _rigidbody;
    private Vector2 _currentVelocity;
    private ShootingManager _shootingManager;
    private PowerLevelManager _powerLevelManager;
    private CharacterHealth _characterHealth;
    private BulletAvoidanceHelper _bulletAvoidanceHelper;
    private Vector2 _desiredLocation;
    private AStar _aStar;
    private bool _disabled = false;
    private float _timeSinceEnabled = 0f;
    private int _randomMoveOffset = 0;

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
        _bulletAvoidanceHelper = GetComponent<BulletAvoidanceHelper>();
        _aStar = new AStar(20, 20, 1);
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

        GetDesiredMoveLocation();
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
        _desiredDistanceFromPlayer = distance;
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

    private void GetDesiredMoveLocation()
    {
        if (Random.value < 0.001)
        {
            var offset = Random.value < 0.5 ? -1 : 1;
            Debug.Log($"Offsetting movement by {offset}. Total offset: {_randomMoveOffset}.");
            _randomMoveOffset += offset;
        }

        var desiredLocation = _playerTarget.position;
        var minDistanceFromPlayer = float.MaxValue;

        const int circlePartitions = 10;
        for (var i = 0; i < circlePartitions; i++)
        {
            var angle = Mathf.PI * 2 * i / circlePartitions;
            var potentialLocation = new Vector2(
                _playerTarget.position.x + _desiredDistanceFromPlayer * Mathf.Cos(angle),
                _playerTarget.position.y + _desiredDistanceFromPlayer * Mathf.Sin(angle));

            var distanceToLocation = Vector2.Distance(transform.position, potentialLocation);
            if (distanceToLocation < minDistanceFromPlayer &&
                HasLineOfSightToPlayer(potentialLocation) &&
                !_bulletAvoidanceHelper.ContainsPlayerBulletsInPosition(potentialLocation))
            {
                minDistanceFromPlayer = distanceToLocation;
                desiredLocation = potentialLocation;
            }
        }

        _desiredLocation = desiredLocation;
    }

    private bool HasLineOfSightToPlayer(Vector2 potentialLocation)
    {
        var hit = Physics2D.Raycast(
            potentialLocation,
            (Vector2)_playerTarget.transform.position - potentialLocation,
            _desiredDistanceFromPlayer,
            LayerMask.GetMask(nameof(CollisionLayer.Obstacle)));

        return hit.collider == null;
    }

    private void SetVelocity()
    {
        if (Mathf.Abs(_desiredLocation.x - transform.position.x) <= 0.5f &&
            Mathf.Abs(_desiredLocation.y - transform.position.y) <= 0.5f)
        {
            _desiredLocation = transform.position;
        }

        var desiredVelocity = (_desiredLocation - (Vector2)transform.position).normalized * _maxVelocity;

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_desiredLocation, 0.5f);
    }
}
