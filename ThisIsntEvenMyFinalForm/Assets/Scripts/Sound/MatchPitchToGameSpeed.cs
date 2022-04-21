using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MatchPitchToGameSpeed : MonoBehaviour
{
    [SerializeField] private EventReference _event;

    private EventInstance _eventInstance;

    void Start()
    {
        _eventInstance = RuntimeManager.CreateInstance(_event);
        _eventInstance.start();
    }

    void Update()
    {
        _eventInstance.setParameterByName("GameSpeed", Time.timeScale);
    }

    private void OnDestroy()
    {
        _eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _eventInstance.release();
    }
}
