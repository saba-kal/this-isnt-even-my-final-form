using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] private float _maxVelocity = 10f;
    [SerializeField] private float _maxForce = 0.25f;
    [SerializeField] private float _desiredDistanceFromPlayer = 1f;
    [SerializeField] private Transform _playerTarget;
    [SerializeField] private GameObject _rotationBody;

    private Rigidbody2D _rigidbody;
    private Vector2 _currentVelocity;
    private ShootingManager _shootingManager;
    private PowerLevelManager _powerLevelManager;
    private Vector2 _desiredLocation;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _shootingManager = GetComponent<ShootingManager>();
        _powerLevelManager = GetComponent<PowerLevelManager>();
    }

    private void FixedUpdate()
    {
        ApplyVelocity();
    }

    void Update()
    {
        var desiredLocation = GetDesiredMoveLocation();
        SetVelocity(desiredLocation);
        FaceSpriteTowardsTarget();
        FireBullets();
    }

    public void SetMaxVelocity(float maxVelocity)
    {
        _maxVelocity = maxVelocity;
    }

    public void SetDistanceFromPlayer(float distance)
    {
        _desiredDistanceFromPlayer = distance;
    }

    private void ApplyVelocity()
    {
        _rigidbody.velocity = _currentVelocity;
    }

    private Vector2 GetDesiredMoveLocation()
    {
        var distanceFromTarget = Vector2.Distance(transform.position, _playerTarget.position);
        var desiredX = (_desiredDistanceFromPlayer / distanceFromTarget) * (transform.position.x - _playerTarget.position.x);
        var desiredY = (_desiredDistanceFromPlayer / distanceFromTarget) * (transform.position.y - _playerTarget.position.y);
        return new Vector2(_playerTarget.position.x + desiredX, _playerTarget.position.y + desiredY);
    }

    private void SetVelocity(Vector2 desiredLocation)
    {
        if (Mathf.Abs(desiredLocation.x - transform.position.x) <= 0.5f &&
            Mathf.Abs(desiredLocation.y - transform.position.y) <= 0.5f)
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
        _shootingManager.FireBulletShooters(CollisionLayer.EnemyBullet);
    }
}
