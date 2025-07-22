using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;

    void Start()
    {
        pauseScreen.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        TimerManager.Instance.PauseTimer();
        pauseScreen.SetActive(true);
    }

    public void ResumeGame()
    {
        TimerManager.Instance.ResumeTimer();
        pauseScreen.SetActive(false);
    }
}
