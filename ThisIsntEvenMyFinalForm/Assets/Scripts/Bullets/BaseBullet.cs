using UnityEngine;
using System.Collections;

public abstract class BaseBullet : MonoBehaviour
{
    [SerializeField] protected int _damage = 1;
    [SerializeField] protected float _speed = 10f;
    [SerializeField] protected float _lifetime = 10f;

    protected Vector2 _direction;

    private void OnEnable()
    {
        LevelMaster.OnStageStart += OnStageStart;
    }

    private void OnDisable()
    {
        LevelMaster.OnStageStart -= OnStageStart;
    }


    void Start()
    {
        VirtualStart();
    }

    protected virtual void VirtualStart()
    {
        Destroy(gameObject, _lifetime);
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private void OnStageStart()
    {
        Destroy(gameObject, 0.5f);
    }
}
