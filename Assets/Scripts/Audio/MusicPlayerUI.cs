using TMPro;
using UnityEngine;
using UnityEngine.UI;

// This script handles the Music Player UI
public class MusicPlayerUI : MonoBehaviour
{
    #region VARIABLES
    [Header("Song Info")]
    [SerializeField] private TextMeshProUGUI songNameTMP;
    [SerializeField] private TextMeshProUGUI artistNameTMP;

    [Header("Pause/Resume Toggle")]
    [SerializeField] private Toggle pauseSongToggle;
    [SerializeField] private Image pauseSongImage;
    [SerializeField] private Sprite pauseSongSprite;
    [SerializeField] private Sprite resumeSongSprite;

    [Header("Loop Toggle")]
    [SerializeField] private Toggle loopSongToggle;
    [SerializeField] private Image loopSongImage;
    [SerializeField] private Sprite loopSongOnSprite;
    [SerializeField] private Sprite loopSongOffSprite;
    #endregion

    // Subscribe to the OnSongChanged event and update the current state of the loop and pause toggle
    private void OnEnable()
    {
        if (AudioManager.Instance == null)
            return;

        bool isPaused = AudioManager.Instance.IsMusicPaused;
        pauseSongToggle.SetIsOnWithoutNotify(isPaused);
        UpdatePauseToggleSprite(isPaused);

        bool isLooping = AudioManager.Instance.IsMusicLoopEnabled;
        loopSongToggle.SetIsOnWithoutNotify(isLooping);
        UpdateLoopToggleSprite(isLooping);

        AudioManager.Instance.OnSongChanged += UpdateSongInfo;

        if (AudioManager.Instance.CurrentSong != null)
            UpdateSongInfo(AudioManager.Instance.CurrentSong);
    }

    // Unsubscribe
    private void OnDisable()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.OnSongChanged -= UpdateSongInfo;
    }

    public void OnNextSongButton()
    {
        AudioManager.Instance.PlayNextSong();
    }

    public void OnPreviousSongButton()
    {
        AudioManager.Instance.PlayPreviousSong();
    }

    public void OnLoopSongToggle(bool isOn)
    {
        AudioManager.Instance.SetMusicLoop(isOn);
        UpdateLoopToggleSprite(isOn);
    }

    public void OnPauseSongToggle(bool isPaused)
    {
        if (isPaused)
        {
            AudioManager.Instance.PauseSong();
        }
        else
        {
            AudioManager.Instance.ResumeSong();
        }

        UpdatePauseToggleSprite(isPaused);
    }

    public void UpdateSongInfo(SoundData song)
    {
        songNameTMP.text = song.songName;
        artistNameTMP.text = song.artistName;
    }

    #region SPRITE UPDATERS
    private void UpdateLoopToggleSprite(bool isOn)
    {
        loopSongImage.sprite = isOn ? loopSongOnSprite : loopSongOffSprite;
    }

    private void UpdatePauseToggleSprite(bool isPaused)
    {
        pauseSongImage.sprite = isPaused ? resumeSongSprite : pauseSongSprite;
    }
    #endregion
}
