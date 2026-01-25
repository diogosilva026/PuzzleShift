using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

// Handles the UI for audio volume and mute settings
public class AudioSettingsUI : MonoBehaviour
{
    #region VARIABLES
    [Header("Volume Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

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
        RegisterPointerUpListeners();
    }

    // Initialize the slider values and toggle states based on the AudioManager
    private void InitializeSlidersAndToggles()
    {
        // Set sliders without triggering any callbacks
        musicSlider.SetValueWithoutNotify(AudioManager.Instance.GetMusicVolume());
        sfxSlider.SetValueWithoutNotify(AudioManager.Instance.GetSFXVolume());

        // Set toggle states
        musicToggle.SetIsOnWithoutNotify(AudioManager.Instance.IsMusicMuted);
        sfxToggle.SetIsOnWithoutNotify(AudioManager.Instance.IsSFXMuted);

        // Set initial icon sprites
        UpdateMusicSprite(AudioManager.Instance.IsMusicMuted);
        UpdateSFXSprite(AudioManager.Instance.IsSFXMuted);
    }

    // Register listeners for UI
    private void RegisterUIListeners()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        musicToggle.onValueChanged.AddListener(ToggleMusicMute);
        sfxToggle.onValueChanged.AddListener(ToggleSFXMute);
    }

    // Register the SaveSettings method to be called when the player releases the cursor on the volume sliders
    private void RegisterPointerUpListeners()
    {
        AddPointerUpListener(musicSlider, SaveSettings);
        AddPointerUpListener(sfxSlider, SaveSettings);
    }

    // Adds a SliderPointerUp component to the given slider
    private void AddPointerUpListener(Slider slider, System.Action action)
    {
        var pointerUp = slider.gameObject.GetComponent<SliderPointerUp>();
        if (pointerUp == null)
            pointerUp = slider.gameObject.AddComponent<SliderPointerUp>();

        pointerUp.onPointerUp = action;
    }

    // Save game action
    private void SaveSettings()
    {
        GameManager.Instance.SaveGame();
    }

    #region VOLUME HANDLERS
    private void SetMusicVolume(float value)
    {
        if (AudioManager.Instance.IsMusicMuted && value > 0f)
        {
            musicToggle.SetIsOnWithoutNotify(false);
            AudioManager.Instance.MuteMusic(false);
            UpdateMusicSprite(false);
        }

        AudioManager.Instance.SetMusicVolume(value);
    }

    private void SetSFXVolume(float value)
    {
        if (AudioManager.Instance.IsSFXMuted && value > 0f)
        {
            sfxToggle.SetIsOnWithoutNotify(false);
            AudioManager.Instance.MuteSFX(false);
            UpdateSFXSprite(false);
        }

        AudioManager.Instance.SetSFXVolume(value);
    }
    #endregion

    #region MUTE TOGGLE HANDLERS
    // When the music mute state is toggled
    public void ToggleMusicMute(bool isMuted)
    {
        AudioManager.Instance.MuteMusic(isMuted);

        if (!isMuted)
            musicSlider.SetValueWithoutNotify(AudioManager.Instance.LastMusicVolume);

        UpdateMusicSprite(isMuted);
        SaveSettings();
    }

    // When the sfx mute state is toggled
    public void ToggleSFXMute(bool isMuted)
    {
        AudioManager.Instance.MuteSFX(isMuted);

        if (!isMuted)
            sfxSlider.SetValueWithoutNotify(AudioManager.Instance.LastSFXVolume);

        UpdateSFXSprite(isMuted);
        SaveSettings();
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
