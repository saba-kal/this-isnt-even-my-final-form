using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float _smoothTime = 0.25f;
    [SerializeField] private Transform _target1;
    [SerializeField] private Transform _target2;

    private Vector3 _velocity = Vector3.zero;

    void LateUpdate()
    {
        var cameraTarget = Vector3.zero;
        if (_target1 != null && _target2 != null)
        {
            //Camera target is the midpoint between target 1 and 2's positions.
            cameraTarget = (_target1.position + _target2.position) / 2;
        }
        else if (_target1 != null)
        {
            cameraTarget = _target1.position;
        }
        else if (_target2 != null)
        {
            cameraTarget = _target2.position;
        }

        var finalTargetPosition = new Vector3(cameraTarget.x, cameraTarget.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, finalTargetPosition, ref _velocity, _smoothTime);
    }
}
