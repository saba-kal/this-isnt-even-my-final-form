using UnityEngine;
using System.Collections;

public class AIMovementHelper : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _desiredDistanceFromPlayer = 1f;
    [SerializeField] private float _timeBetweenLocationChange = 1f;
    [SerializeField] private GameObject _desiredMoveLocationObject;
    [SerializeField] private GameObject _actualMoveLocationObject;
    [SerializeField] private Transform _playerTarget;

    private BulletAvoidanceHelper _bulletAvoidanceHelper;
    private float _timeSinceLastLocationChange = 0f;

    void Awake()
    {
        _bulletAvoidanceHelper = GetComponent<BulletAvoidanceHelper>();
    }

    // Update is called once per frame
    void Update()
    {
        SetDesiredDistanceFromPlayer();
        CalculateDesiredLocation();
        RotateMoveLocationTowardsDesiredLocation();
    }

    public void SetDistanceFromPlayer(float distance)
    {
        _desiredDistanceFromPlayer = distance;
    }

    public Vector2 GetMoveLocation()
    {
        return _actualMoveLocationObject.transform.GetChild(0).position;
    }

    private void SetDesiredDistanceFromPlayer()
    {
        var desiredTarget = _desiredMoveLocationObject.transform.GetChild(0);
        var actualTarget = _actualMoveLocationObject.transform.GetChild(0);

        desiredTarget.transform.localPosition = new Vector2(0, _desiredDistanceFromPlayer);
        actualTarget.transform.localPosition = new Vector2(0, _desiredDistanceFromPlayer);
    }

    private void CalculateDesiredLocation()
    {
        if (_timeSinceLastLocationChange < _timeBetweenLocationChange)
        {
            _timeSinceLastLocationChange += Time.deltaTime;
            return;
        }

        _timeSinceLastLocationChange = 0f;
        var desiredAngle = 0f;
        var minDistanceFromPlayer = float.MaxValue;
        const int circlePartitions = 10;
        for (var i = 0; i < circlePartitions; i++)
        {
            var angle = 360f * i / circlePartitions;
            _desiredMoveLocationObject.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);

            var potentialLocation = _desiredMoveLocationObject.transform.GetChild(0).position;
            var distanceToLocation = Vector2.Distance(transform.position, potentialLocation);
            if (distanceToLocation < minDistanceFromPlayer &&
                HasLineOfSightToPlayer(potentialLocation) &&
                !_bulletAvoidanceHelper.ContainsPlayerBulletsInPosition(potentialLocation))
            {
                minDistanceFromPlayer = distanceToLocation;
                desiredAngle = angle;
            }
        }

        _desiredMoveLocationObject.transform.localRotation = Quaternion.AngleAxis(desiredAngle, Vector3.forward);
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

    private void RotateMoveLocationTowardsDesiredLocation()
    {
        _actualMoveLocationObject.transform.localRotation = Quaternion.RotateTowards(
            _actualMoveLocationObject.transform.localRotation,
            _desiredMoveLocationObject.transform.localRotation,
            _rotationSpeed * Time.deltaTime);
    }
}
