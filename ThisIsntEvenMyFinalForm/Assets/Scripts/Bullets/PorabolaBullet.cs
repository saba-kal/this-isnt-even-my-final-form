using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorabolaBullet : BaseBullet
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _arcHeight;

    private Vector2 _startPosition;
    private Vector2 _target;
    private float _stepScale;
    private float _progress;

    protected override void VirtualStart()
    {
        base.VirtualStart();
        SetupParabola();
    }

    private void Update()
    {
        MoveInParabola();
        Rotate();
    }

    //Source: https://gamedev.stackexchange.com/questions/183507/add-parabola-curve-to-straight-movetowards-movement
    private void MoveInParabola()
    {
        // Increment our progress from 0 at the start, to 1 when we arrive.
        _progress = Mathf.Min(_progress + Time.deltaTime * _stepScale, 1.0f);

        // Turn this 0-1 value into a parabola that goes from 0 to 1, then back to 0.
        var parabola = 1.0f - 4.0f * (_progress - 0.5f) * (_progress - 0.5f);

        // Travel in a straight line from our start position to the target.        
        var nextPos = Vector2.Lerp(_startPosition, _target, _progress);

        // Then add a vertical arc in excess of this.
        nextPos.x += parabola * _arcHeight;

        // Continue as before.
        transform.position = nextPos;

        // We arrived at our destination.
        if (_progress == 1.0f)
        {
            _arcHeight = -_arcHeight / 1.2f;
            _speed = _speed / 1.2f;
            SetupParabola();
        }
    }

    private void SetupParabola()
    {
        _startPosition = transform.position;
        _target = PlayerController.Instance.transform.position;
        _stepScale = _speed / Vector3.Distance(_startPosition, _target);
        _progress = 0f;
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
    }
}
