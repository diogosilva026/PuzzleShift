using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer; // Refer�ncia ao Audio Mixer

    [Header("Audio Clips")]
    public AudioClip backgroundMusic; // M�sica de fundo
    public AudioClip applauseSFX; // Efeito sonoro de aplausos

    private AudioSource musicSource; // Para a m�sica de fundo
    private AudioSource sfxSource; // Para os efeitos sonoros

    void Awake()
    {
        // Garantir que h� apenas um AudioManager na cena
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre cenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Criar e configurar os AudioSources
        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        // Configurar M�sica
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
            musicSource.volume = 0.5f; // Volume inicial
            musicSource.Play();
        }

        // Configurar SFX
        sfxSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
        sfxSource.volume = 0.5f;
    }

    /// <summary>
    /// Toca um efeito sonoro espec�fico
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Define o volume da m�sica (ligado ao slider)
    /// </summary>
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
    }

    /// <summary>
    /// Define o volume dos efeitos sonoros (ligado ao slider)
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
    }

    /// <summary>
    /// Inicia a m�sica de fundo
    /// </summary>
    public void PlayMusic(AudioClip musicClip)
    {
        if (musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.Play();
        }
    }

    /// <summary>
    /// Para a m�sica atual
    /// </summary>
    public void StopMusic()
    {
        musicSource.Stop();
    }
}
