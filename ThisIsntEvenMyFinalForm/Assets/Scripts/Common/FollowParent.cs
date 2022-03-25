using UnityEngine;
using System.Collections;

public class FollowParent : MonoBehaviour
{
    [SerializeField] private float _lifetimeAfterParentDeath;

    private Transform _parent;
    private bool _isBeingDestroyed = false;

    // Use this for initialization
    void Awake()
    {
        _parent = transform.parent;
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (_parent != null)
        {
            transform.position = _parent.position;
        }
        else if (!_isBeingDestroyed)
        {
            Destroy(gameObject, _lifetimeAfterParentDeath);
            _isBeingDestroyed = true;
        }
    }
}
