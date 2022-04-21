using UnityEngine;
using System.Collections;
using FMODUnity;
using FMOD.Studio;

public class GameMusicManager : MonoBehaviour
{
    [SerializeField] private EventReference _event;

    private EventInstance _eventInstance;
    private const string PARAMETER_NAME = "GameState";

    void Awake()
    {
        _eventInstance = RuntimeManager.CreateInstance(_event);
        _eventInstance.setParameterByName(PARAMETER_NAME, 1);
        _eventInstance.start();
    }

    private void OnEnable()
    {
        LevelMaster.OnGameStateChange += OnGameStateChange;
    }

    private void OnDisable()
    {
        LevelMaster.OnGameStateChange -= OnGameStateChange;
        _eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void OnGameStateChange(int gameState)
    {
        _eventInstance.setParameterByName(PARAMETER_NAME, gameState);
    }
}
