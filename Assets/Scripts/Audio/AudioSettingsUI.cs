using UnityEngine;
using UnityEngine.UI;

// This script handles the UI for audio volume settings
public class AudioSettingsUI : MonoBehaviour
{
    #region VARIABLES
    [Header("Volume Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private float lastMusicSliderValue = 1f;
    [SerializeField] private float lastSFXSliderValue = 1f;

    [Header("Music Mute Toggle")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Image musicImage;

    [Header("SFX Mute Toggle")]
    [SerializeField] private Toggle sfxToggle;
    [SerializeField] private Image sfxImage;

    [Header("Toggle Sprites")]
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    #endregion

    private void Start()
    {
        InitializeSlidersAndToggles();
        RegisterUIListeners();
    }

    // Initialize the slider values and toggle states based on the AudioManager
    private void InitializeSlidersAndToggles()
    {
        float musicDb = AudioManager.Instance.GetMusicVolume();
        float sfxDb = AudioManager.Instance.GetSFXVolume();

        float musicValue = DbToSlider(musicDb);
        float sfxValue = DbToSlider(sfxDb);

        bool musicMuted = AudioManager.Instance.IsMusicMuted || musicDb <= -79.9f;
        bool sfxMuted = AudioManager.Instance.IsSFXMuted || sfxDb <= -79.9f;

        // Set sliders without triggering any callbacks
        musicSlider.SetValueWithoutNotify(musicValue);
        sfxSlider.SetValueWithoutNotify(sfxValue);

        // Set toggle states
        musicToggle.SetIsOnWithoutNotify(musicMuted);
        sfxToggle.SetIsOnWithoutNotify(sfxMuted);

        // Set initial icon sprites
        UpdateMusicSprite(musicMuted);
        UpdateSFXSprite(sfxMuted);
    }

    // Register listeners for UI
    private void RegisterUIListeners()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        musicToggle.onValueChanged.AddListener(ToggleMusicMute);
        sfxToggle.onValueChanged.AddListener(ToggleSFXMute);
    }

    #region AUDIO CONVERSION METHODS
    // Convert a slider value to dB
    private float SliderToDb(float sliderValue)
    {
        sliderValue = Mathf.Clamp(sliderValue, 0.0001f, 1f);
        return Mathf.Log10(sliderValue) * 20f;
    }

    // Convert a dB value to a slider range value
    private float DbToSlider(float dbValue)
    {
        return Mathf.Pow(10f, dbValue / 20f);
    }
    #endregion

    #region VOLUME HANDLERS
    // Set music volume based on slider value
    private void SetMusicVolume(float sliderValue)
    {
        float dbValue = SliderToDb(sliderValue);

        // If the user unmutes music via slider, updates the toggle and sprite
        if (AudioManager.Instance.IsMusicMuted && dbValue > -80f)
        {
            musicToggle.SetIsOnWithoutNotify(false);
            AudioManager.Instance.MuteMusic(false);
            UpdateMusicSprite(false);
        }

        AudioManager.Instance.SetMusicVolume(dbValue);
        lastMusicSliderValue = sliderValue;
    }

    // Set SFX volume based on slider value
    private void SetSFXVolume(float sliderValue)
    {
        float dbValue = SliderToDb(sliderValue);

        // If the user unmutes sfx via slider, updates the toggle and sprite
        if (AudioManager.Instance.IsSFXMuted && dbValue > -80f)
        {
            sfxToggle.SetIsOnWithoutNotify(false);
            AudioManager.Instance.MuteSFX(false);
            UpdateSFXSprite(false);
        }

        AudioManager.Instance.SetSFXVolume(dbValue);
        lastSFXSliderValue = sliderValue;
    }
    #endregion

    #region MUTE TOGGLE HANDLERS
    // When the music mute state is toggled
    public void ToggleMusicMute(bool isMuted)
    {
        if (isMuted)
        {
            lastMusicSliderValue = musicSlider.value;
            AudioManager.Instance.MuteMusic(true);
        }
        else
        {
            AudioManager.Instance.MuteMusic(false);
            musicSlider.SetValueWithoutNotify(lastMusicSliderValue);
            AudioManager.Instance.SetMusicVolume(SliderToDb(lastMusicSliderValue));
        }

        UpdateMusicSprite(isMuted);
    }

    // When the sfx mute state is toggled
    public void ToggleSFXMute(bool isMuted)
    {
        if (isMuted)
        {
            lastSFXSliderValue = sfxSlider.value;
            AudioManager.Instance.MuteSFX(true);
        }
        else
        {
            AudioManager.Instance.MuteSFX(false);
            sfxSlider.SetValueWithoutNotify(lastSFXSliderValue);
            AudioManager.Instance.SetSFXVolume(SliderToDb(lastSFXSliderValue));
        }

        UpdateSFXSprite(isMuted);
    }
    #endregion

    #region SPRITE UPDATERS
    private void UpdateMusicSprite(bool isMuted)
    {
        musicImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
    }

    private void UpdateSFXSprite(bool isMuted)
    {
        sfxImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
    }
    #endregion
}
