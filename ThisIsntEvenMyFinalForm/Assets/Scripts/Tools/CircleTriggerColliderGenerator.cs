using UnityEngine;
using System.Collections;

public class CircleTriggerColliderGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] private float _radius;
    [SerializeField] private int _divisions;

    public void Generate()
    {
        for (var i = 0; i < _divisions; i++)
        {
            var angle1 = Mathf.PI * 2 * i / _divisions;
            var angle2 = Mathf.PI * 2 * (i + 1) / _divisions;

            var points = new[] { (Vector2)_container.transform.position, GetPointOnCircle(angle1), GetPointOnCircle(angle2) };

            var triggerColliderObject = new GameObject($"TriggerCollider ({i + 1})");
            triggerColliderObject.transform.parent = _container.transform;

            var collider = triggerColliderObject.AddComponent<PolygonCollider2D>();
            collider.isTrigger = true;
            collider.points = points;

            triggerColliderObject.AddComponent<BulletAvoidanceCollider>();
        }
    }

    private Vector2 GetPointOnCircle(float angle)
    {
        return new Vector2(
                _container.transform.position.x + _radius * Mathf.Cos(angle),
                _container.transform.position.y + _radius * Mathf.Sin(angle));
    }
}
