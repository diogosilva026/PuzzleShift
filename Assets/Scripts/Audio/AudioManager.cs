using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    #region SINGLETON
    public static AudioManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region VARIABLES
    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private List<SoundData> musicTracks;
    [SerializeField] private int currentSongIndex = -1;

    private bool wasMusicPlaying = false;
    public bool IsMusicLoopEnabled { get; private set; }
    public bool IsMasterMuted { get; private set; }
    public bool IsMusicMuted { get; private set; }
    public bool IsSFXMuted { get; private set; }
    public float LastMasterVolume { get; private set; } = 1f;
    public float LastMusicVolume { get; private set; } = 1f;
    public float LastSFXVolume { get; private set; } = 1f;

    private Dictionary<string, SoundData> soundCache = new();
    #endregion

    private void Start()
    {
        if (currentSongIndex < 0 || currentSongIndex >= musicTracks.Count)
        {
            currentSongIndex = Random.Range(0, musicTracks.Count);
        }

        PlaySong(musicTracks[currentSongIndex]);
    }

    private void Update()
    {
        if (IsMusicLoopEnabled) return;

        // Detect song end
        if (wasMusicPlaying && !musicSource.isPlaying)
        {
            PlayNextSong();
        }

        wasMusicPlaying = musicSource.isPlaying;
    }

    #region VOLUME

    #region SETTERS
    public void SetMasterVolume(float linearValue)
    {
        LastMasterVolume = linearValue;
        mixer.SetFloat("MasterVolume", LinearToDb(linearValue));
    }

    public void SetMusicVolume(float linearValue)
    {
        LastMusicVolume = linearValue;
        mixer.SetFloat("MusicVolume", LinearToDb(linearValue));
    }

    public void SetSFXVolume(float linearValue)
    {
        LastSFXVolume = linearValue;
        mixer.SetFloat("SFXVolume", LinearToDb(linearValue));
    }
    #endregion

    #region GETTERS
    public float GetMasterVolume() => LastMasterVolume;

    public float GetMusicVolume() => LastMusicVolume;

    public float GetSFXVolume() => LastSFXVolume;
    #endregion

    #region AUDIO CONVERSION
    private float LinearToDb(float linear) => Mathf.Approximately(linear, 0f) ? -80f : Mathf.Log10(linear) * 20f;

    private float DbToLinear(float dB) => Mathf.Pow(10f, dB / 20f);
    #endregion

    #region MUTE
    public void MuteMusic(bool mute)
    {
        IsMusicMuted = mute;
        mixer.SetFloat("MusicVolume", mute ? -80f : LinearToDb(LastMusicVolume));
    }

    public void MuteSFX(bool mute)
    {
        IsSFXMuted = mute;
        mixer.SetFloat("SFXVolume", mute ? -80f : LinearToDb(LastSFXVolume));
    }
    #endregion

    #endregion

    #region SFX
    // Plays the desired sfx once
    public void PlaySFX(string soundName)
    {
        SoundData sound = LoadSFX(soundName);
        if (sound == null || sound.clip == null) return;

        // Creates a temporary gameobject holding the audiosource needed
        GameObject tempGO = new($"SFX_{soundName}");
        AudioSource source = tempGO.AddComponent<AudioSource>();

        source.outputAudioMixerGroup = sound.mixerGroup != null ? sound.mixerGroup : sfxGroup;
        source.volume = sound.volume;
        source.pitch = sound.pitch;
        source.PlayOneShot(sound.clip);

        Destroy(tempGO, sound.clip.length + 0.1f);
    }

    // Plays the desired sfx once but with a random pitch
    public void PlaySFXWithRandomPitch(string soundName)
    {
        SoundData sound = LoadSFX(soundName);
        if (sound == null || sound.clip == null) return;

        GameObject tempGO = new($"SFX_{soundName}");
        AudioSource source = tempGO.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = sound.mixerGroup != null ? sound.mixerGroup : sfxGroup;
        source.volume = sound.volume;
        source.pitch = sound.pitch + Random.Range(-0.1f, 0.1f);
        source.PlayOneShot(sound.clip);

        Destroy(tempGO, sound.clip.length + 0.1f);
    }

    // Loads SoundData for SFX only, caching it so it only loads once from the resources
    private SoundData LoadSFX(string soundName)
    {
        if (soundCache.TryGetValue(soundName, out var cached)) return cached;

        SoundData loaded = Resources.Load<SoundData>($"Audio/SFX/{soundName}");
        if (loaded != null)
            soundCache[soundName] = loaded;

        return loaded;
    }
    #endregion

    #region MUSIC
    public void PlaySong(SoundData music)
    {
        if (music == null || music.clip == null) return;

        musicSource.clip = music.clip;
        musicSource.volume = music.volume;
        musicSource.pitch = music.pitch;
        musicSource.loop = IsMusicLoopEnabled;
        musicSource.outputAudioMixerGroup = music.mixerGroup != null ? music.mixerGroup : musicGroup;

        musicSource.Play();
        wasMusicPlaying = true;
    }

    private void PlayNextSong()
    {
        currentSongIndex++;

        // Wrap around playlist
        if (currentSongIndex >= musicTracks.Count)
            currentSongIndex = 0;

        PlaySong(musicTracks[currentSongIndex]);
    }

    // Enable or disable music loop
    public void SetMusicLoop(bool enabled)
    {
        IsMusicLoopEnabled = enabled;

        if (musicSource != null)
        {
            musicSource.loop = enabled;
        }
    }
    #endregion
}
