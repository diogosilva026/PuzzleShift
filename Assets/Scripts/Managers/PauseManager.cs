using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private bool isGamePaused = false;

    void Start()
    {
        pauseScreen.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        TimerManager.Instance.PauseTimer();
        pauseScreen.SetActive(true);
        isGamePaused = true;
    }

    public void ResumeGame()
    {
        TimerManager.Instance.ResumeTimer();
        pauseScreen.SetActive(false);
        isGamePaused = false;
    }
}
