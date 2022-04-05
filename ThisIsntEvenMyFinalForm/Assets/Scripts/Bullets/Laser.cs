using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _timeBetweenDamage;
    [SerializeField] private LineRenderer _laserRenderer;
    [SerializeField] private GameObject _laserStartEffect;
    [SerializeField] private GameObject _laserEndEffect;

    private float _timeSinceLastDamage = 0f;

    private void Update()
    {
        FireLaser();
        _timeSinceLastDamage += Time.deltaTime;
    }

    private void FireLaser()
    {
        _laserRenderer.SetPosition(0, transform.position);
        _laserStartEffect.transform.position = transform.position;

        var hit = Physics2D.Raycast(
            transform.position,
            transform.right,
            100f,
            LayerMask.GetMask(nameof(CollisionLayer.Obstacle)) |
            LayerMask.GetMask(nameof(CollisionLayer.Player)));

        var laserEndPoint = transform.TransformPoint(new Vector2(100, 0));
        if (hit.collider != null)
        {
            laserEndPoint = hit.point;
            if (_timeSinceLastDamage >= _timeBetweenDamage)
            {
                var health = hit.collider.GetComponentInChildren<Health>();
                health?.TakeDamage(_damage);
                _timeSinceLastDamage = 0f;
            }
        }

        _laserRenderer.SetPosition(1, laserEndPoint);
        _laserEndEffect.transform.position = laserEndPoint;
    }
}
