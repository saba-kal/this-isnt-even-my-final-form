using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private GameObject _bulletPrefab;

    public void Fire(Vector2 direction, CollisionLayer layer)
    {
        var bullet = Instantiate(_bulletPrefab);
        bullet.layer = (int)layer;
        bullet.transform.position = transform.position;

        var rigidBody = bullet.GetComponent<Rigidbody2D>();
        rigidBody.velocity = direction.normalized * _bulletSpeed;

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }
}
