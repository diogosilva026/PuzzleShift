using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audio/SoundData")]
public class SoundData : ScriptableObject
{
    public string soundName;
    public AudioClip clip;
    public AudioMixerGroup mixerGroup;
    public float volume = 1f;
    public float pitch = 1f;
    public bool loop = false;
}
