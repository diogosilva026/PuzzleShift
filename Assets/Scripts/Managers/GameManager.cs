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
                    return addedStars;
                }
            }
            else
            {
                bestLevelStars.Add(levelName, newStars);
                UpdateTotalPlayerStars(newStars);
            }

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

        #region TIMER STUFF
        // Saves the best time for each level in the dictionary, but only if it is faster than the previous one
        public bool SaveLevelTime(string levelName, float time)
        {
            if (bestLevelCompletionTimes.ContainsKey(levelName))
            {
                if (time < bestLevelCompletionTimes[levelName])
                {
                    bestLevelCompletionTimes[levelName] = time;
                    return true; // New best time
                }
                return false; // Not a new best time
            }
            else
            {
                bestLevelCompletionTimes.Add(levelName, time); // Adds the level and completed time if it is not already in the dictionary
                return true; // First time for this level - considered a new best
            }
        }

        // Returns the best time for each level
        public float GetLevelTime(string levelName) => bestLevelCompletionTimes.TryGetValue(levelName, out float time) ? time : 0f;
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