using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private TextMeshProUGUI ingameTimerTMP;
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

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        ingameTimerTMP.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Pauses the timer
    public void PauseTimer() => timeIsRunning = false;

    // Resumes the timer
    public void ResumeTimer() => timeIsRunning = true;

    // Saves and stores the time after finishing a level
    public void EndTimer()
    {
        timeIsRunning = false;
        
        bool isNewBest = GameManager.Instance.SaveLevelTime(currentLevel, timer);

        if (isNewBest)
        {
            // Trigger UI feedback
        }
    }
}