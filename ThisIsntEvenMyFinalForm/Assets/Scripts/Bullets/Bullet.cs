﻿using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

public class Bullet : BaseBullet
{
    [SerializeField] protected GameObject _onDestroyEffect;
    [SerializeField] protected bool _shakeCameraOnDestory;
    [SerializeField] protected bool _destroyIfCollidingObjectNotDestroyed;

    private float _timeSinceSpawn = 0f;

    void Update()
    {
        VirtualUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var otherObjectsHealth = collision.gameObject.GetComponentInChildren<Health>();
        if (otherObjectsHealth != null)
        {
            var otherObjectWasDestroyed = otherObjectsHealth.TakeDamage(_damage);
            if (_destroyIfCollidingObjectNotDestroyed &&
                !otherObjectWasDestroyed)
            {
                Destroy(gameObject);
            }
        }

        if (collision.gameObject.layer != (int)CollisionLayer.EnemyBullet &&
            collision.gameObject.layer != (int)CollisionLayer.PlayerBullet &&
            !CollisionIsFromSpawningOnTopOfObstacle())
        {
            //Colliding with a non-bullet object should destroy this bullet.
            Destroy(gameObject);
        }

        StartCoroutine(IncreaseLightBrightness());
    }

    private void OnDestroy()
    {
        if (_shakeCameraOnDestory)
        {
            CinemachineShake.Instance.Shake();
        }

        if (_onDestroyEffect == null || !gameObject.scene.isLoaded)
        {
            return;
        }

        var effect = Instantiate(_onDestroyEffect);
        effect.transform.position = transform.position;
    }

    protected virtual void VirtualUpdate()
    {
        transform.Translate(_direction.normalized * _speed * Time.deltaTime, Space.World);
        _timeSinceSpawn += Time.deltaTime;
    }

    private bool CollisionIsFromSpawningOnTopOfObstacle()
    {
        if (_timeSinceSpawn > 0.1f)
        {
            return false;
        }

        var hit = Physics2D.Raycast(transform.position, _direction, 2f, LayerMask.GetMask(nameof(CollisionLayer.Obstacle)));
        return hit.collider == null;
    }

    private IEnumerator IncreaseLightBrightness()
    {
        //TODO: move this to separate component.
        var lights = GetComponentsInChildren<Light2D>();

        foreach (var light in lights)
        {
            light.intensity *= 2;
        }

        yield return new WaitForSeconds(0.2f);

        foreach (var light in lights)
        {
            light.intensity /= 2;
        }

        yield return null;
    }
}
