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

        RotateDoor(_door1, -1);
        RotateDoor(_door2, 1);

        if (_doorOpenFinished)
        {
            _openDoor = false;
            _onDoorOpenFinished?.Invoke();
        }
    }

    private void RotateDoor(GameObject door, int direction)
    {
        door.transform.rotation = Quaternion.AngleAxis(
            direction * _doorOpenSpeed * Time.deltaTime,
            Vector3.forward);

        if (Mathf.Abs(door.transform.rotation.eulerAngles.z) >= _rotationAmount)
        {
            _doorOpenFinished = true;
        }
    }
}
