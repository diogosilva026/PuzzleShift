using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
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


    public void QuitGame()
    {
        Application.Quit();
    }
}
