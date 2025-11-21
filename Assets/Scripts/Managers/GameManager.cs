using System.IO;
using UnityEngine;

// Using this namespace so I can serialize dictionaries
namespace AYellowpaper.SerializedCollections
{
    public class GameManager : MonoBehaviour
    {
        #region SINGLETON
        public static GameManager Instance { get; private set; }

        private void Awake()
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
        [Header("Stars Stuff")]
        private int totalGameStars = 27;
        [SerializeField] private int totalPlayerStars = 0;

        [SerializedDictionary("Level Name", "Star Thresholds")]
        public SerializedDictionary<string, StarThreshold> levelStarThresholds;

        [SerializedDictionary("Level Name", "Best Stars Earned")]
        public SerializedDictionary<string, int> bestLevelStars;

        [Header("Timer Stuff")]
        [SerializedDictionary("Level Name", "Best Time")]
        public SerializedDictionary<string, float> bestLevelCompletionTimes;

        [Header("Game Save Stuff")]
        private string savePath;

        [Header("Settings Stuff")]
        public bool IsFullscreen = true;
        #endregion

        private void Start()
        {
            savePath = Path.Combine(Application.persistentDataPath, "save.json");
            LoadGame();
        }

        #region GAME SAVE STUFF
        // Saves the player's progress into a json file on disk
        public void SaveGame()
        {
            SaveData data = new()
            {
                totalPlayerStars = totalPlayerStars,
                bestLevelStars = new SerializedDictionary<string, int>(bestLevelStars),
                bestLevelCompletionTimes = new SerializedDictionary<string, float>(bestLevelCompletionTimes),
                musicVolume = AudioManager.Instance.GetMusicVolume(),
                sfxVolume = AudioManager.Instance.GetSFXVolume(),
                isMusicMuted = AudioManager.Instance.IsMusicMuted,
                isSFXMuted = AudioManager.Instance.IsSFXMuted,
                isFullscreen = IsFullscreen
            };

            // Converts the SaveData object into a json string
            string json = JsonUtility.ToJson(data, true);

            // Writes the json string into a file at "savePath"
            File.WriteAllText(savePath, json);
        }

        // Loads the player's progress into the game from the json file
        public void LoadGame()
        {
            if (File.Exists(savePath))
            {
                // Reads all text from the save file
                string json = File.ReadAllText(savePath);

                // Converts the json text back into SaveData object
                SaveData data = JsonUtility.FromJson<SaveData>(json);

                // Restores the saved data back into the GameManager
                totalPlayerStars = data.totalPlayerStars;
                bestLevelStars = new SerializedDictionary<string, int>(data.bestLevelStars);
                bestLevelCompletionTimes = new SerializedDictionary<string, float>(data.bestLevelCompletionTimes);

                AudioManager.Instance.SetMusicVolume(data.musicVolume);
                AudioManager.Instance.SetSFXVolume(data.sfxVolume);
                AudioManager.Instance.MuteMusic(data.isMusicMuted);
                AudioManager.Instance.MuteSFX(data.isSFXMuted);

                IsFullscreen = data.isFullscreen;
            }
        }

        public void DeleteProgress()
        {
            // Reset all player progress (excluding the audio and settings)
            totalPlayerStars = 0;
            bestLevelStars.Clear();
            bestLevelCompletionTimes.Clear();

            // Keep audio variables as they are
            float musicVol = AudioManager.Instance.GetMusicVolume();
            float sfxVol = AudioManager.Instance.GetSFXVolume();
            bool musicMuted = AudioManager.Instance.IsMusicMuted;
            bool sfxMuted = AudioManager.Instance.IsSFXMuted;

            // Keep settings variables as they are
            bool fullscreen = IsFullscreen;

            SaveData data = new()
            {
                totalPlayerStars = totalPlayerStars,
                bestLevelStars = new SerializedDictionary<string, int>(bestLevelStars),
                bestLevelCompletionTimes = new SerializedDictionary<string, float>(bestLevelCompletionTimes),
                musicVolume = musicVol,
                sfxVolume = sfxVol,
                isMusicMuted = musicMuted,
                isSFXMuted = sfxMuted,
                isFullscreen = fullscreen
            };

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);
        }
        #endregion

        #region TIMER STUFF
        // Saves the best time for each level in the dictionary, but only if it is faster than the previous one
        public bool SaveLevelTime(string levelName, float time)
        {
            if (bestLevelCompletionTimes.ContainsKey(levelName))
            {
                if (time < bestLevelCompletionTimes[levelName])
                {
                    bestLevelCompletionTimes[levelName] = time;
                    SaveGame();
                    return true; // New best time
                }
                return false; // Not a new best time
            }
            else
            {
                bestLevelCompletionTimes.Add(levelName, time); // Adds the level and completed time if it is not already in the dictionary
                SaveGame();
                return true; // First time for this level - considered a new best
            }
        }

        // Returns the best time for each level
        public float GetLevelTime(string levelName) => bestLevelCompletionTimes.TryGetValue(levelName, out float time) ? time : 0f;
        #endregion

        #region STARS STUFF

        #region TOTAL STARS
        // Returns the maximum stars the player can have
        public int GetTotalGameStars() => totalGameStars;

        // Returns the total stars collected by the player
        public int GetTotalPlayerStars() => totalPlayerStars;

        // Changes the number of total stars collected
        public void UpdateTotalPlayerStars(int value) => totalPlayerStars += value;
        #endregion

        #region STARS THRESHOLD
        // Calculates if a level completion time is worth 2 or 3 stars, 1 is guaranteed
        public int CalculateLevelTimeStars(string levelName, float completionTime)
        {
            if (levelStarThresholds.TryGetValue(levelName, out var thresholds))
            {
                if (completionTime <= thresholds.timeFor3Stars)
                    return 3;
                if (completionTime <= thresholds.timeFor2Stars)
                    return 2;
                else
                    return 1;
            }
            return 1;
        }
        #endregion

        #region LEVEL STARS
        // Updates the total number of unique stars the player's earned
        public int UpdateStarsForLevel(string levelName, int newStars)
        {
            if (bestLevelStars.TryGetValue(levelName, out int previousBest))
            {
                if (newStars > previousBest)
                {
                    bestLevelStars[levelName] = newStars;
                    int addedStars = newStars - previousBest;
                    UpdateTotalPlayerStars(addedStars);

                    SaveGame();
                    return addedStars;
                }
            }
            else
            {
                bestLevelStars.Add(levelName, newStars);
                UpdateTotalPlayerStars(newStars);

                SaveGame();
            }

            SaveGame();
            return 0;
        }

        // Returns the number of stars collected on a certain level
        public int GetStarsForLevel(string levelName) => bestLevelStars.TryGetValue(levelName, out int stars) ? stars : 0;

        // Updates the stored star count for a specific level in bestLevelStars disctionary
        public void SetStarsForLevelUI(string levelName, int starsEarned)
        {
            if (bestLevelStars.ContainsKey(levelName))
            {
                bestLevelStars[levelName] = Mathf.Max(bestLevelStars[levelName], starsEarned);
            }
            else
            {
                bestLevelStars.Add(levelName, starsEarned);
            }
        }
        #endregion

        #endregion
    }
}

[System.Serializable]
public class StarThreshold
{
    public float timeFor2Stars;
    public float timeFor3Stars;

    public StarThreshold(float time2, float time3)
    {
        timeFor2Stars = time2;
        timeFor3Stars = time3;
    }
}