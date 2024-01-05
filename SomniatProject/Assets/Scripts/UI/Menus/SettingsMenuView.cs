using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class SettingsMenuView : Menu
{
    [SerializeField] private Button _backButton;

    public AudioMixer audioMixer;

    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    Resolution[] resolutions;

    public TMPro.TMP_Dropdown resolutionDropdown;
    private void Awake()
    {
        DontDestroyOnLoad(this);

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for(int i=0; i<resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);

        masterVolumeSlider.value = AudioManager.master;
        musicVolumeSlider.value = AudioManager.music;
        sfxVolumeSlider.value = AudioManager.sfx;

    }
    public override void Initialize()
    {
        _backButton.onClick.AddListener(() => MainMenuManager.ShowLast());
    }

    public void SetMusicVolume(float volume)
    {
        AudioManager.music = volume;
    }

    public void SetMasterVolume(float volume)
    {
        AudioManager.master = volume;
    }

    public void SetSFXVolume(float volume)
    {
        AudioManager.sfx = volume;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
