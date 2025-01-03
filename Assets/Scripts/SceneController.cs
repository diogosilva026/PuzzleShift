using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // This script has the purpose to change between scenes
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    #region EASY LEVELS

    public void PlayEasy1()
    {
        SceneManager.LoadScene("Easy1");
    }
    public void PlayEasy2()
    {
        SceneManager.LoadScene("Easy2");
    }
    public void PlayEasy3()
    {
        SceneManager.LoadScene("Easy3");
    }

    #endregion

    #region MEDIUM LEVELS

    public void PlayMedium1()
    {
        SceneManager.LoadScene("Medium1");
    }
    public void PlayMedium2()
    {
        SceneManager.LoadScene("Medium2");
    }
    public void PlayMedium3()
    {
        SceneManager.LoadScene("Medium3");
    }

    #endregion

    #region HARD LEVELS

    public void PlayHard1()
    {
        SceneManager.LoadScene("Hard1");
    }
    public void PlayHard2()
    {
        SceneManager.LoadScene("Hard2");
    }
    public void PlayHard3()
    {
        SceneManager.LoadScene("Hard3");
    }

    #endregion
}
