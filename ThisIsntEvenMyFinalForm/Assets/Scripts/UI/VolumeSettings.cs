using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;

    private VCA _masterVCA;
    private VCA _musicVCA;
    private VCA _sfxVCA;

    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    void Start()
    {
        //_masterVCA = RuntimeManager.GetVCA("vca:/Master");
        //_musicVCA = RuntimeManager.GetVCA("vca:/Music");
        //_sfxVCA = RuntimeManager.GetVCA("vca:/SFX");
        SetupVolumeSlider(_masterVolumeSlider, MASTER_VOLUME_KEY, RuntimeManager.GetVCA("vca:/Master"));
        SetupVolumeSlider(_musicVolumeSlider, MUSIC_VOLUME_KEY, RuntimeManager.GetVCA("vca:/Music"));
        SetupVolumeSlider(_sfxVolumeSlider, SFX_VOLUME_KEY, RuntimeManager.GetVCA("vca:/SFX"));
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, _masterVolumeSlider.value);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, _musicVolumeSlider.value);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, _sfxVolumeSlider.value);
    }

    private void SetupVolumeSlider(
        Slider slider,
        string volumeKey,
        VCA vca)
    {
        slider.value = PlayerPrefs.GetFloat(volumeKey, 0.5f);
        vca.setVolume(slider.value);
        slider.onValueChanged.AddListener((value) =>
        {
            vca.setVolume(value);
        });
    }
}
