using UnityEngine;
using System.Collections;

public abstract class BaseBullet : MonoBehaviour
{
    [SerializeField] protected int _damage = 1;
    [SerializeField] protected float _speed = 10f;
    [SerializeField] protected float _lifetime = 10f;
    [SerializeField] protected string _bulletTrailName;

    protected Vector2 _direction;

    private void OnEnable()
    {
        LevelMaster.OnStageStart += OnStageStart;
    }

    private void OnDisable()
    {
        LevelMaster.OnStageStart -= OnStageStart;
    }

    public virtual void Initialize()
    {
        Invoke("DisableBullet", _lifetime);
        BulletTrailManager.Instance?.EnableTrail(_bulletTrailName, gameObject);
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private void OnStageStart()
    {
        Invoke("DisableBullet", 0.5f);
    }

    public void DisableBullet()
    {
        gameObject.SetActive(false);
    }
}
