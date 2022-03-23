using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private GameObject _bulletPrefab;

    private float _timeSinceLastFire = 0;

    private void Update()
    {
        _timeSinceLastFire += Time.deltaTime;
    }

    public void Fire(CollisionLayer layer)
    {
        if (_timeSinceLastFire < _fireRate)
        {
            return;
        }

        var bullet = Instantiate(_bulletPrefab);
        bullet.layer = (int)layer;
        bullet.transform.position = transform.position;

        var bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent?.SetDirection(transform.right);

        _timeSinceLastFire = 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.TransformPoint(Vector2.right));
    }
}
