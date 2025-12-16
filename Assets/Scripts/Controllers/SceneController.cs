using UnityEngine;
using UnityEngine.SceneManagement;

// This script has the purpose to change between scenes
public class SceneController : MonoBehaviour
{
    #region EASY LEVELS
    public void LoadEasy1() => SceneManager.LoadScene("Easy1");
    public void LoadEasy2() => SceneManager.LoadScene("Easy2");
    public void LoadEasy3() => SceneManager.LoadScene("Easy3");
    public void LoadEasy4() => SceneManager.LoadScene("Easy4");
    public void LoadEasy5() => SceneManager.LoadScene("Easy5");
    public void LoadEasy6() => SceneManager.LoadScene("Easy6");
    #endregion

    #region MEDIUM LEVELS
    public void LoadMedium1() => SceneManager.LoadScene("Medium1");
    public void LoadMedium2() => SceneManager.LoadScene("Medium2");
    public void LoadMedium3() => SceneManager.LoadScene("Medium3");
    public void LoadMedium4() => SceneManager.LoadScene("Medium4");
    public void LoadMedium5() => SceneManager.LoadScene("Medium5");
    public void LoadMedium6() => SceneManager.LoadScene("Medium6");
    #endregion

    #region HARD LEVELS
    public void LoadHard1() => SceneManager.LoadScene("Hard1");
    public void LoadHard2() => SceneManager.LoadScene("Hard2");
    public void LoadHard3() => SceneManager.LoadScene("Hard3");
    public void LoadHard4() => SceneManager.LoadScene("Hard4");
    public void LoadHard5() => SceneManager.LoadScene("Hard5");
    public void LoadHard6() => SceneManager.LoadScene("Hard6");
    #endregion

    public void LoadMainMenu() => SceneManager.LoadScene("MainMenu");

    // Closes the game
    public void QuitGame() => Application.Quit();
}