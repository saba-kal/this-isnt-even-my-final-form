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

    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    void Start()
    {
        SetupVolumeSlider(_masterVolumeSlider, MASTER_VOLUME_KEY);
        SetupVolumeSlider(_musicVolumeSlider, MUSIC_VOLUME_KEY);
        SetupVolumeSlider(_sfxVolumeSlider, SFX_VOLUME_KEY);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, _masterVolumeSlider.value);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, _musicVolumeSlider.value);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, _sfxVolumeSlider.value);
    }

    private void SetupVolumeSlider(
        Slider slider,
        string volumeKey)
    {
        slider.value = PlayerPrefs.GetFloat(volumeKey, 0.5f);
        _audioMixer.SetFloat(volumeKey, Mathf.Log10(slider.value) * 20f);
        slider.onValueChanged.AddListener((value) =>
        {
            _audioMixer.SetFloat(volumeKey, Mathf.Log10(value) * 20f);
        });
    }
}
