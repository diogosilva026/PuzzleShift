using AYellowpaper.SerializedCollections;

[System.Serializable]
public class SaveData
{
    public int totalPlayerStars;
    public SerializedDictionary<string, int> bestLevelStars;
    public SerializedDictionary<string, float> bestLevelCompletionTimes;
}
