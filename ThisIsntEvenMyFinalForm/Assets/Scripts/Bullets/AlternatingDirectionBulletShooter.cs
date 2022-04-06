using UnityEngine;
using System.Collections;

public class AlternatingDirectionBulletShooter : BulletShooter
{
    [SerializeField] private float _rotationOnEachFire = 10f;


    public override void Fire(CollisionLayer layer)
    {
        transform.Rotate(new Vector3(0f, 0f, _rotationOnEachFire));
        base.Fire(layer);
    }
}
