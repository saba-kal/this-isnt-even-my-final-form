using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletAvoidanceCollider : MonoBehaviour
{
    private List<GameObject> _bulletsInsideTrigger = new List<GameObject>();
    private bool _containsAICharacter = false;
    private PolygonCollider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<PolygonCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)CollisionLayer.Enemy)
        {
            _containsAICharacter = true;
        }

        if (collision.gameObject.layer == (int)CollisionLayer.PlayerBullet &&
            !_bulletsInsideTrigger.Contains(collision.gameObject))
        {
            _bulletsInsideTrigger.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)CollisionLayer.Enemy)
        {
            _containsAICharacter = false;
        }

        if (_bulletsInsideTrigger.Contains(collision.gameObject))
        {
            _bulletsInsideTrigger.Remove(collision.gameObject);
        }
    }

    public bool ContainsPlayerBulletsInPosition(Vector2 position)
    {
        return _bulletsInsideTrigger.Count > 0 &&
            _collider.bounds.Contains(position);
    }
}
