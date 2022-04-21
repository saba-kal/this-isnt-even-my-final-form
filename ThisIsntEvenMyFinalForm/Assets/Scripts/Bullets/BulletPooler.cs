using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Object pooler for managing the creation and destruction of bullets.
/// </summary>
public class BulletPooler
{
    private Queue<GameObject> _bulletQueue;

    private readonly GameObject _bulletPrefab;
    private readonly int _capacity;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="bulletPrefab">Prefab of the bullet to pool.</param>
    /// <param name="capacity">Maximum number of bullets to instantiate.</param>
    public BulletPooler(
        GameObject bulletPrefab,
        int capacity)
    {
        _bulletPrefab = bulletPrefab;
        _capacity = capacity;
    }

    /// <summary>
    /// Initializes the queue of bullets in an inactive state.
    /// </summary>
    public void CreatePool()
    {
        _bulletQueue = new Queue<GameObject>();

        for (var i = 0; i < _capacity; i++)
        {
            var bullet = Object.Instantiate(_bulletPrefab);
            bullet.gameObject.SetActive(false);
            _bulletQueue.Enqueue(bullet);
        }
    }

    /// <summary>
    /// Creates a bullet from the obect pool.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public GameObject CreateBullet(
        Vector2 position,
        Quaternion rotation)
    {
        var bullet = _bulletQueue.Dequeue();
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.gameObject.SetActive(true);
        EnableChildObjects(bullet);

        _bulletQueue.Enqueue(bullet);

        return bullet;
    }

    /// <summary>
    /// Destroys all bullets stored in this pooler.
    /// </summary>
    public void Cleanup()
    {
        foreach (var bullet in _bulletQueue)
        {
            Object.Destroy(bullet);
        }

        _bulletQueue = new Queue<GameObject>();
    }

    private void EnableChildObjects(GameObject gameObj)
    {
        foreach (Transform child in gameObj.transform)
        {
            child.gameObject.SetActive(true);
            EnableChildObjects(child.gameObject);
        }
    }
}
