using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    public GameObject[] finalPattern;
    public GameObject[] possibleObjects;
    public GameObject[] currentPattern;
    public GameObject[] patternPositions;

    private int confirmedPatternCount;
    private PuzzlePositions puzzlePos;

    void Start()
    {
        currentPattern = new GameObject[finalPattern.Length];

        for (int i = 0; i < patternPositions.Length; i++)
        {
            puzzlePos = patternPositions[i].GetComponent<PuzzlePositions>();
            puzzlePos.patternID = i + 1;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (currentPattern != null)
        {
            CheckAssignedObject();

            confirmedPatternCount = 0;
            CheckPattern();
        }
        if (confirmedPatternCount >= finalPattern.Length)
        {
            SceneManager.LoadScene("Win");
        }
    }

    void CheckPattern()
    {
        for (int i = 0; i < finalPattern.Length; i++)
        {
            if (currentPattern[i] == finalPattern[i])
            {
                confirmedPatternCount++;
            }
        }
    }

    void CheckAssignedObject()
    {
        for (int i = 0; i < finalPattern.Length; i++)
        {
            currentPattern[i] = patternPositions[i].GetComponent<PuzzlePositions>().assignedObject;
        }
    }
}
