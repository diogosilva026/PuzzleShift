using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip backgroundMusic; // Assign the audio clip in the inspector
    public AudioClip applauseSFX; // Applause sound effect
    private static AudioManager instance; // Singleton instance
    private AudioSource audioSource;

    void Awake()
    {
        // Ensure only one instance of the AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        // Set up the audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.volume = 0.5f; // Adjust volume if needed
        audioSource.Play(); // Start playing music
    }

    // Method to play applause sound
    public void PlayApplause()
    {
        if (applauseSFX != null)
        {
            audioSource.PlayOneShot(applauseSFX);
        }
    }
}
