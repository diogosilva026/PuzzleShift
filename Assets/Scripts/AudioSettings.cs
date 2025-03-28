using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        if (AudioManager.instance != null)
        {
            // Inicializa os sliders com o valor atual do volume
            float musicVolume, sfxVolume;
            AudioManager.instance.audioMixer.GetFloat("MusicVolume", out musicVolume);
            AudioManager.instance.audioMixer.GetFloat("SFXVolume", out sfxVolume);

            musicSlider.value = Mathf.Pow(10, musicVolume / 20);
            sfxSlider.value = Mathf.Pow(10, sfxVolume / 20);

            // Adiciona os listeners para atualizar o volume quando o slider muda
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (AudioManager.instance != null)
            AudioManager.instance.SetMusicVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (AudioManager.instance != null)
            AudioManager.instance.SetSFXVolume(volume);
    }
}
