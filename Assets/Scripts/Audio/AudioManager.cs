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

    public bool IsMusicMuted { get; private set; }
    public bool IsSFXMuted { get; private set; }

    private Dictionary<string, SoundData> soundCache = new();
    #endregion

    void Start()
    {
        // Temporary while there is no save system
        if (musicTracks.Count == 0) return;

        if (currentSongIndex < 0 || currentSongIndex >= musicTracks.Count)
        {
            currentSongIndex = Random.Range(0, musicTracks.Count);
        }

        // Not temporary
        PlayMusic(musicTracks[currentSongIndex]);
    }

    #region VOLUME
    public void SetMasterVolume(float value) => mixer.SetFloat("MasterVolume", value);
    public void SetMusicVolume(float value) => mixer.SetFloat("MusicVolume", value);
    public void SetSFXVolume(float value) => mixer.SetFloat("SFXVolume", value);

    public float GetMasterVolume() => GetVolume("MasterVolume");
    public float GetMusicVolume() => GetVolume("MusicVolume");
    public float GetSFXVolume() => GetVolume("SFXVolume");

    private float GetVolume(string parameter)
    {
        mixer.GetFloat(parameter, out float value);
        return value;
    }

    public void MuteMusic(bool mute)
    {
        IsMusicMuted = mute;
        mixer.SetFloat("MusicVolume", mute ? -80f : 0f);
    }

    public void MuteSFX(bool mute)
    {
        IsSFXMuted = mute;
        mixer.SetFloat("SFXVolume", mute ? -80f : 0f);
    }
    #endregion

    #region SFX
    // Plays the desired sfx once
    public void PlaySFX(string soundName)
    {
        SoundData sound = LoadSound(soundName);
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
        SoundData sound = LoadSound(soundName);
        if (sound == null || sound.clip == null) return;

        GameObject tempGO = new($"SFX_{soundName}");
        AudioSource source = tempGO.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = sound.mixerGroup != null ? sound.mixerGroup : sfxGroup;
        source.volume = sound.volume;
        source.pitch = sound.pitch + Random.Range(-0.1f, 0.1f);
        source.PlayOneShot(sound.clip);

        Destroy(tempGO, sound.clip.length + 0.1f);
    }
    #endregion

    #region MUSIC
    public void PlayMusic(SoundData music, bool loop = true)
    {
        if (music == null || music.clip == null) return;

        musicSource.clip = music.clip;
        musicSource.volume = music.volume;
        musicSource.pitch = music.pitch;
        musicSource.loop = loop;
        musicSource.outputAudioMixerGroup = music.mixerGroup != null ? music.mixerGroup : musicGroup;

        musicSource.Play();
    }
    #endregion

    #region LOADING
    // Loads a SoundData asset, caching it so it only loads once from the resources
    private SoundData LoadSound(string soundName)
    {
        if (soundCache.TryGetValue(soundName, out var cached)) return cached;

        SoundData loaded = Resources.Load<SoundData>($"Audio/{soundName}");
        if (loaded != null)
            soundCache[soundName] = loaded;

        return loaded;
    }
    #endregion
}
