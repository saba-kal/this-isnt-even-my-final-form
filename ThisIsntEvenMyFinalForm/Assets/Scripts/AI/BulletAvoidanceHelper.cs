using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BulletAvoidanceHelper : MonoBehaviour
{
    [SerializeField] private GameObject _colliderHelpersContainer;

    private List<BulletAvoidanceCollider> _colliders;

    void Start()
    {
        _colliders = _colliderHelpersContainer.GetComponentsInChildren<BulletAvoidanceCollider>().ToList();
    }

    public bool ContainsPlayerBulletsInPosition(Vector2 position)
    {
        return _colliders.Any(c => c.ContainsPlayerBulletsInPosition(position));
    }
}
