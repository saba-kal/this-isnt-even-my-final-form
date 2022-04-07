using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _timeBetweenDamage;
    [SerializeField] private float _maxDistance = 100f;
    [SerializeField] private LineRenderer _laserRenderer;
    [SerializeField] private GameObject _laserStartEffect;
    [SerializeField] private GameObject _laserEndEffect;

    private float _timeSinceLastDamage = 0f;

    private void OnEnable()
    {
        SoundManager.Instance?.Play(SoundClipNames.LASER_SFX, true);
    }

    private void OnDisable()
    {
        SoundManager.Instance?.Stop(SoundClipNames.LASER_SFX);
    }

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
            _maxDistance,
            LayerMask.GetMask(nameof(CollisionLayer.Obstacle)) |
            LayerMask.GetMask(nameof(CollisionLayer.Player)));

        var laserEndPoint = transform.TransformPoint(new Vector2(_maxDistance, 0));
        if (hit.collider != null)
        {
            laserEndPoint = hit.point;
            if (_timeSinceLastDamage >= _timeBetweenDamage &&
                 hit.collider.gameObject.layer == (int)CollisionLayer.Player)
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
