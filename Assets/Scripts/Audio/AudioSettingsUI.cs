using UnityEngine;
using UnityEngine.UI;

// This script handles the UI for audio volume settings
public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        // Get the current volume from AudioManager (dB) and convert it to slider value
        musicSlider.value = DbToSlider(AudioManager.Instance.GetMusicVolume());
        sfxSlider.value = DbToSlider(AudioManager.Instance.GetSFXVolume());

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    // Set music volume based on slider value
    private void SetMusicVolume(float volume)
    {
        AudioManager.Instance.SetMusicVolume(SliderToDb(volume));
    }

    // Set SFX volume based on slider value
    private void SetSFXVolume(float volume)
    {
        AudioManager.Instance.SetSFXVolume(SliderToDb(volume));
    }

    // Convert a slider value to dB
    private float SliderToDb(float sliderValue)
    {
        return Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20f;
    }

    // Convert a dB value to a slider range value
    private float DbToSlider(float dbValue)
    {
        return Mathf.Pow(10f, dbValue / 20f);
    }
}
