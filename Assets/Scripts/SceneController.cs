using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script has the purpose to change between scenes
public class SceneController : MonoBehaviour
{
    #region EASY LEVELS
    public void LoadEasy1() => SceneManager.LoadScene("Easy1");
    public void LoadEasy2() => SceneManager.LoadScene("Easy2");
    public void LoadEasy3() => SceneManager.LoadScene("Easy3");
    #endregion

    #region MEDIUM LEVELS
    public void LoadMedium1() => SceneManager.LoadScene("Medium1");
    public void LoadMedium2() => SceneManager.LoadScene("Medium2");
    public void LoadMedium3() => SceneManager.LoadScene("Medium3");
    #endregion

    #region HARD LEVELS
    public void LoadHard1() => SceneManager.LoadScene("Hard1");
    public void LoadHard2() => SceneManager.LoadScene("Hard2");
    public void LoadHard3() => SceneManager.LoadScene("Hard3");
    #endregion

    public void LoadMainMenu() => SceneManager.LoadScene("MainMenu");

    // Closes the game
    public void QuitGame() => Application.Quit();
}