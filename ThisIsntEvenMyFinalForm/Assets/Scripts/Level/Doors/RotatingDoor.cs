using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingDoor : Door
{
    [SerializeField] private float _rotationAmount = 90;
    [SerializeField] private GameObject _door1;
    [SerializeField] private GameObject _door2;

    protected bool _doorOpenFinished = false;

    void Update()
    {
        if (!_openDoor)
        {
            return;
        }

        RotateDoor(_door1, 1);
        RotateDoor(_door2, -1);

        if (_doorOpenFinished)
        {
            _openDoor = false;
            _onDoorOpenFinished?.Invoke();
        }
    }

    private void RotateDoor(GameObject door, int direction)
    {
        door.transform.localRotation = Quaternion.AngleAxis(
            door.transform.localEulerAngles.z + direction * _doorOpenSpeed * Time.deltaTime,
            Vector3.forward);

        if (GetRotationAngle(door) >= _rotationAmount)
        {
            _doorOpenFinished = true;
        }
    }

    private float GetRotationAngle(GameObject door)
    {
        var absoluteAngle = Mathf.Abs(door.transform.localEulerAngles.z);
        if (absoluteAngle > 180)
        {
            return 360 - absoluteAngle;
        }
        return absoluteAngle;
    }
}
