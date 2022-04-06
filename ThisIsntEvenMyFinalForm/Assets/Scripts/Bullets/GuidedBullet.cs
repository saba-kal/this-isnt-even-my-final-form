using UnityEngine;
using System.Collections;

public class GuidedBullet : Bullet
{
    [SerializeField] private float _followTurnSpeed = 2f;

    private Transform _target;

    protected override void VirtualStart()
    {
        base.VirtualStart();

        if (gameObject.layer == (int)CollisionLayer.EnemyBullet)
        {
            _target = PlayerController.Instance?.transform;
        }
        if (gameObject.layer == (int)CollisionLayer.PlayerBullet)
        {
            _target = AIController.Instance?.transform;
        }
    }

    protected override void VirtualUpdate()
    {
        if (_target != null)
        {
            var desiredDirection = (_target.position - transform.position).normalized;
            _direction = Vector3.RotateTowards(_direction, desiredDirection, Time.deltaTime * _followTurnSpeed, 1f);
        }


        base.VirtualUpdate();
    }
}
