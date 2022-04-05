using UnityEngine;
using System.Collections;

public class FollowParent : MonoBehaviour
{
    [SerializeField] private float _lifetimeAfterParentDeath;

    private Transform _parent;
    private bool _isBeingDestroyed = false;
    private ParticleSystem _particleSystem;

    // Use this for initialization
    void Awake()
    {
        _parent = transform.parent;
        transform.parent = null;
        _particleSystem = GetComponent<ParticleSystem>();
        _particleSystem?.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (_particleSystem != null && !_particleSystem.isPlaying)
        {
            _particleSystem?.Play();
        }

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
