using System.Collections;
using System.Collections.Generic;
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
        [Header("Player Stats")]
        [SerializeField] private int totalStars = 0;

        [Header("Easy Levels")]
        [SerializeField] private bool isEasy1Complete = false;
        [SerializeField] private bool isEasy2Complete = false;
        [SerializeField] private bool isEasy3Complete = false;

        [Header("Medium Levels")]
        [SerializeField] private bool isMedium1Complete = false;
        [SerializeField] private bool isMedium2Complete = false;
        [SerializeField] private bool isMedium3Complete = false;

        [Header("Hard Levels")]
        [SerializeField] private bool isHard1Complete = false;
        [SerializeField] private bool isHard2Complete = false;
        [SerializeField] private bool isHard3Complete = false;

        [Header("Timer Stuff")]
        [SerializedDictionary("Level Name", "Best Time")]
        public SerializedDictionary<string, float> bestLevelCompletionTimes;
        #endregion

        #region PLAYER STATS
        // Returns the total stars collected
        public int GetTotalStars() => totalStars;

        // Changes the number of total stars collected
        public void UpdateTotalStars(int value)
        {
            totalStars += value;
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
                    return true; // New best time
                }
                return false; // Not a new best time
            }
            else
            {
                // Adds the level and completed time if it is not already in the dictionary
                bestLevelCompletionTimes.Add(levelName, time);
                return true; // First time for this level - considered a new best
            }
        }

        // Returns the best time for each level
        public float GetLevelTime(string levelName) => bestLevelCompletionTimes.TryGetValue(levelName, out float time) ? time : 0f;
        #endregion
    }
}