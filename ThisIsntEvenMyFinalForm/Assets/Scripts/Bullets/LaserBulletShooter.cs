using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserBulletShooter : BulletShooter
{
    [SerializeField] private List<Laser> _lasers;

    private void OnEnable()
    {
        LevelMaster.OnStageStart += OnStageStart;
    }

    private void OnDisable()
    {
        LevelMaster.OnStageStart -= OnStageStart;
    }

    public override void Initialize()
    {
        SetLasersActive(false);
    }

    public override void Fire(CollisionLayer layer)
    {
        SetLasersActive(true);
    }

    public override void Cleanup()
    {
        SetLasersActive(false);
    }

    private void OnStageStart()
    {
        SetLasersActive(false);
    }

    private void SetLasersActive(bool active)
    {
        foreach (var laser in _lasers)
        {
            laser.gameObject.SetActive(active);
        }
    }
}
