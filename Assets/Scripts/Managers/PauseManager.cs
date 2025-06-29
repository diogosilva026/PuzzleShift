using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PauseGame()
    {
        TimerManager.Instance.PauseTimer();
    }

    public void ResumeGame()
    {
        TimerManager.Instance.ResumeTimer();
    }
}
