using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsHandler : MonoBehaviour
{
    #region SINGLETON
    public static GameSettingsHandler Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    [Header("Fullscreen Stuff")]
    [SerializeField] private Toggle fullscreenToggle;

    public void SetFullscreen(bool isFullscreen)
    {
        // Store the value
        GameManager.Instance.IsFullscreen = isFullscreen;

        if (isFullscreen)
        {
            // Go fullscreen with native resolution
            Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
        }
        else
        {
            // Go windowed with default setting
            Screen.fullScreen = false;
        }
        
        // Update toggle UI
        fullscreenToggle.isOn = isFullscreen;
    }

    public void UpdateSettingsUI()
    {
        // Update toggle UI
        fullscreenToggle.isOn = GameManager.Instance.IsFullscreen;
    }

    public void ApplyChanges()
    {
        GameManager.Instance.SaveGame();
    }
}