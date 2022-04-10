using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] private BulletShooterType _type = BulletShooterType.Normal;
    [SerializeField] private int _bulletCount = 1;
    [SerializeField] private float _angleSpread = 15;
    [SerializeField] private float _horizontalSpread = 0;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private int _bulletPoolCapacity = 10;
    [SerializeField] private GameObject _bulletPrefab;

    private float _timeSinceLastFire = 0;
    private Action _onFire = null;
    private BulletPooler _bulletPooler;

    private void Update()
    {
        VirtualUpdate();
    }

    protected virtual void VirtualUpdate()
    {
        _timeSinceLastFire += Time.deltaTime;
    }

    public virtual void Initialize()
    {
        _bulletPooler = new BulletPooler(_bulletPrefab, _bulletPoolCapacity);
        _bulletPooler.CreatePool();
    }

    public virtual void Fire(CollisionLayer layer)
    {
        if (_timeSinceLastFire < _fireRate)
        {
            return;
        }

        var angleOffset = _angleSpread * (_bulletCount - 1) / 2;
        var positionOffset = _horizontalSpread * (_bulletCount - 1) / 2;
        for (int i = 0; i < _bulletCount; i++)
        {
            //Get start position
            var position = transform.localPosition;
            position.y += i * _horizontalSpread - positionOffset;

            //Get direction
            var direction = Quaternion.AngleAxis(i * _angleSpread - angleOffset, Vector3.forward) * transform.right;

            var bullet = _bulletPooler.CreateBullet(transform.TransformPoint(position), Quaternion.identity);

            var bulletComponent = bullet.GetComponent<BaseBullet>();
            bulletComponent?.SetDirection(direction);
            bulletComponent?.Initialize();
        }

        _timeSinceLastFire = 0f;
        _onFire?.Invoke();
        PlaySound();
    }

    public bool IsType(BulletShooterType type)
    {
        return type.HasFlag(_type);
    }

    public float GetFireRate()
    {
        return _fireRate;
    }

    public void SetOnFire(Action onFire)
    {
        _onFire = onFire;
    }

    public virtual void Cleanup()
    {
        _bulletPooler.Cleanup();
    }

    private void PlaySound()
    {
        if (_type == BulletShooterType.Heavy)
        {
            SoundManager.Instance?.Play(SoundClipNames.LARGE_BULLET_SFX);
        }
        else if (gameObject.layer == (int)CollisionLayer.Enemy)
        {
            SoundManager.Instance?.Play(SoundClipNames.ENEMY_BULLET_SFX);
        }
        else
        {
            SoundManager.Instance?.Play(SoundClipNames.SMALL_BULLET_SFX);
        }
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
