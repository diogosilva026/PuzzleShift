using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsHandler : MonoBehaviour
{
    [Header("Fullscreen Stuff")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Button deleteProgressButton;

    private void Start()
    {
        deleteProgressButton.onClick.AddListener(GameManager.Instance.DeleteProgress);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        // Store value on the GameManager
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
    }

    public void UpdateSettingsUI()
    {
        fullscreenToggle.isOn = GameManager.Instance.IsFullscreen;
    }

    public void ApplyChanges()
    {
        GameManager.Instance.SaveGame();
    }
}