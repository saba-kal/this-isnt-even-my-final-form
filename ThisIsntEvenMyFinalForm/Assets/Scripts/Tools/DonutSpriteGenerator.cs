using UnityEngine;
using System.Collections;

public class DonutSpriteGenerator : MonoBehaviour
{
    [SerializeField] private float _divisions;
    [SerializeField] private float _radius;
    [SerializeField] private GameObject _squareSprite;
    [SerializeField] private GameObject _container;

    public void Generate()
    {
        for (var i = 0; i < _container.transform.childCount; i++)
        {
            DestroyImmediate(_container.transform.GetChild(i).gameObject);
        }

        var donutContainer = new GameObject("Donut");
        donutContainer.transform.parent = _container.transform;
        for (var i = 0; i < _divisions; i++)
        {
            var angle = Mathf.PI * 2 * i / _divisions;
            var point = GetPointOnCircle(angle);

            var square = Instantiate(_squareSprite, donutContainer.transform);
            square.gameObject.name = $"Square ({i})";
            square.transform.position = point;
            square.transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg - 90, Vector3.forward);
        }
    }

    private Vector2 GetPointOnCircle(float angle)
    {
        return new Vector2(
                _container.transform.position.x + _radius * Mathf.Cos(angle),
                _container.transform.position.y + _radius * Mathf.Sin(angle));
    }
}
