using UnityEngine;
using System.Collections;
using FMODUnity;
using FMOD.Studio;

public class PlayWhileEnabled : MonoBehaviour
{

    [SerializeField] private EventReference _event;

    private EventInstance _eventInstance;

    void Awake()
    {
        _eventInstance = RuntimeManager.CreateInstance(_event);
    }

    void OnEnable()
    {
        _eventInstance.start();
    }

    void OnDisable()
    {
        _eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    void OnDestroy()
    {
        _eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _eventInstance.release();
    }
}
