using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifetime = 10f;

    private Vector2 _direction;

    void Start()
    {
        Destroy(gameObject, _lifetime);
    }

    void Update()
    {
        transform.Translate(_direction.normalized * _speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var health = collision.gameObject.GetComponent<Health>();
        health?.TakeDamage(_damage);

        Destroy(gameObject);
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
}
