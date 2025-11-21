using AYellowpaper.SerializedCollections;

[System.Serializable]
public class SaveData
{
    public int totalPlayerStars;
    public SerializedDictionary<string, int> bestLevelStars;
    public SerializedDictionary<string, float> bestLevelCompletionTimes;

    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public bool isMusicMuted = false;
    public bool isSFXMuted = false;

    public bool isFullscreen = false;
}