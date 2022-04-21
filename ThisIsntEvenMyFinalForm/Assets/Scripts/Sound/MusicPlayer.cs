using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private EventReference _event;

    private EventInstance _eventInstance;

    void Start()
    {
        _eventInstance = RuntimeManager.CreateInstance(_event);
        _eventInstance.start();
    }

    private void OnDestroy()
    {
        _eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _eventInstance.release();
    }
}
