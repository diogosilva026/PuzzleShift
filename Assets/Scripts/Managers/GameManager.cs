using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region SINGLETON
    public static GameManager Instance {  get; private set; }

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
    #endregion

    #region PLAYER STATS
    // Devolve o numero total de estrelas
    public int GetTotalStars() => totalStars;

    // Altera o valor do numero total de estrelas
    public void UpdateTotalStars(int value)
    {
        totalStars += value;
    }
    #endregion
}
