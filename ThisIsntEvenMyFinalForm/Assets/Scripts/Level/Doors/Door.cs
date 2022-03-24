using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Door : MonoBehaviour
{
    [SerializeField] protected float _doorOpenSpeed;

    protected bool _openDoor = false;
    protected Action _onDoorOpenFinished;

    public void OpenDoor(Action onDoorOpenFinished = null)
    {
        _openDoor = true;
        _onDoorOpenFinished = onDoorOpenFinished;
    }
}
