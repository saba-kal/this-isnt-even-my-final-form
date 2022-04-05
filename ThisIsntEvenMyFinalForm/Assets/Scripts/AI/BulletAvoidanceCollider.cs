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

    private void OnDrawGizmos()
    {
        if (_collider == null ||
            _collider.GetPath(0) == null ||
            _collider.GetPath(0).Length < 3)
        {
            return;
        }

        var mesh = new Mesh();

        mesh.vertices = new Vector3[3]
        {
            _collider.GetPath(0)[0],
            _collider.GetPath(0)[1],
            _collider.GetPath(0)[2]
        };

        mesh.triangles = new int[]
        {
            2, 1, 0
        };

        mesh.normals = new Vector3[]
        {
            Vector3.back,
            Vector3.back,
            Vector3.back
        };

        var color = Color.blue;
        color.a = _bulletsInsideTrigger.Count / 20f;
        Gizmos.color = color;

        Gizmos.DrawMesh(mesh, transform.position);
    }
}
