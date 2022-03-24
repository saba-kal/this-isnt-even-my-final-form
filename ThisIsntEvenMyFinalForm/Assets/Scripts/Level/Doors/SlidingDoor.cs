using UnityEngine;
using System.Collections;

public class SlidingDoor : Door
{
    [SerializeField] private GameObject _leftDoor;
    [SerializeField] private GameObject _rightDoor;

    protected bool _doorOpenFinished = false;

    void Update()
    {
        if (!_openDoor)
        {
            return;
        }

        SlideDoor(_leftDoor, -1);
        SlideDoor(_rightDoor, 1);

        if (_doorOpenFinished)
        {
            _openDoor = false;
            Destroy(_leftDoor);
            Destroy(_rightDoor);
            _onDoorOpenFinished?.Invoke();
        }
    }

    private void SlideDoor(GameObject door, int direction)
    {
        var scale = door.transform.localScale;
        var position = door.transform.localPosition;

        scale.x -= _doorOpenSpeed * Time.deltaTime;
        position.x += direction * _doorOpenSpeed * Time.deltaTime / 2;

        if (scale.x <= 0)
        {
            scale.x = 0;
            _doorOpenFinished = true;
        }

        door.transform.localScale = scale;
        door.transform.localPosition = position;
    }
}
