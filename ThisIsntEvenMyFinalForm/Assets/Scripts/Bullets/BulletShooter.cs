using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] private int _bulletCount = 1;
    [SerializeField] private float _angleSpread = 15;
    [SerializeField] private float _horizontalSpread = 0;
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


        var angleOffset = _angleSpread * (_bulletCount - 1) / 2;
        var positionOffset = _horizontalSpread * (_bulletCount - 1) / 2;
        for (int i = 0; i < _bulletCount; i++)
        {
            //Create bullet.
            var bullet = Instantiate(_bulletPrefab);
            bullet.layer = (int)layer;

            //Set start position
            var position = transform.localPosition;
            position.y += i * _horizontalSpread - positionOffset;
            bullet.transform.position = transform.TransformPoint(position);

            //Set direction
            var direction = Quaternion.AngleAxis(i * _angleSpread - angleOffset, Vector3.forward) * transform.right;
            var bulletComponent = bullet.GetComponent<Bullet>();
            bulletComponent?.SetDirection(direction);
        }

        _timeSinceLastFire = 0f;
    }

    private void OnDrawGizmos()
    {
        var angleOffset = _angleSpread * (_bulletCount - 1) / 2;
        var positionOffset = _horizontalSpread * (_bulletCount - 1) / 2;
        Gizmos.color = Color.green;
        for (int i = 0; i < _bulletCount; i++)
        {
            var direction = Quaternion.AngleAxis(i * _angleSpread - angleOffset, Vector3.forward) * Vector2.right;

            var position = transform.localPosition;
            position.y += i * _horizontalSpread - positionOffset;

            var endPoint = position + direction;

            Gizmos.DrawLine(transform.TransformPoint(position), transform.TransformPoint(endPoint));
        }
    }
}
