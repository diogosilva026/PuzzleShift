using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class TimerManager : MonoBehaviour
{
    #region SINGLETON
    public static TimerManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        currentLevel = SceneManager.GetActiveScene().name;
    }
    #endregion

    #region VARIABLES
    [SerializeField] private TextMeshProUGUI levelTimerTMP;
    [SerializeField] private TextMeshProUGUI endLevelTimeTMP;
    [SerializeField] private GameObject newBestText;
    [SerializeField] private float timer = 0f;
    [SerializeField] private bool timeIsRunning = false;
    [SerializeField] private string currentLevel;
    #endregion

    private void Update()
    {
        if (timeIsRunning)
        {
            timer += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    // Displays the timer UI in minutes, seconds and hundredths
    private void UpdateTimerDisplay()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
        levelTimerTMP.text = timeSpan.ToString(@"mm\:ss\.ff");
    }

    // Starts the timer when starting a level
    public void StartTimer()
    {
        timeIsRunning = true;
        timer = 0f;
    }

    // Pauses the timer
    public void PauseTimer() => timeIsRunning = false;

    // Resumes the timer
    public void ResumeTimer() => timeIsRunning = true;

    // Saves and stores the time after finishing a level
    public void EndTimer()
    {
        timeIsRunning = false;

        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
        endLevelTimeTMP.text = timeSpan.ToString(@"mm\:ss\.ff");
        
        bool isNewBest = GameManager.Instance.SaveLevelTime(currentLevel, timer);
        newBestText.SetActive(isNewBest);

        int starsEarned = GameManager.Instance.CalculateLevelTimeStars(currentLevel, timer);
        GameManager.Instance.UpdateStarsForLevel(currentLevel, starsEarned);

        StarsUIHandler starsUIHandler = FindObjectOfType<StarsUIHandler>();
        starsUIHandler.DisplayLevelEndingStars(starsEarned);
    }
}