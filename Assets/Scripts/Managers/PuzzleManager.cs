using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    #region VARIABLES
    private PuzzleLevelData levelData;
    [SerializeField] private GameObject puzzlePiecePrefab;
    [SerializeField] private GameObject targetSquarePrefab;
    [SerializeField] private List<GameObject> spawnedPieces = new();
    private List<TargetSquare> allTargetSquaresList = new();
    #endregion

    private void Start()
    {
        LoadLevelData();
        SpawnTargetSquares();
        SpawnPuzzlePieces();

        DragController dragController = FindObjectOfType<DragController>();
        if (dragController != null )
        {
            dragController.validTargets = allTargetSquaresList;
        }
    }

    // Loads the chosen puzzle/level data, connecting its json file with the PuzzleLevelData class
    private void LoadLevelData()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        TextAsset jsonFile = Resources.Load<TextAsset>($"PuzzleConfigs/{sceneName}");

        if (jsonFile != null)
        {
            levelData = JsonUtility.FromJson<PuzzleLevelData>(jsonFile.text);
        }
        else
        {
            Debug.LogError($"JSON file for scene {sceneName} not found!");
        }
    }

    #region SPAWN STUFF FROM JSON
    // Spawns the Puzzle Pieces depending on the json setting
    private void SpawnPuzzlePieces()
    {
        // Creates a copy of the spawn points and shuffles them
        List<Vector3> shuffledSpawnPoints = new(levelData.pieceSpawnPoints);
        ShuffleList(shuffledSpawnPoints);

        for (int i = 0; i < levelData.pieceCount; i++)
        {
            Vector3 spawnPos = shuffledSpawnPoints[i];

            GameObject pieceObj = Instantiate(puzzlePiecePrefab, spawnPos, Quaternion.identity);

            pieceObj.layer = LayerMask.NameToLayer("PuzzlePiece");

            pieceObj.transform.localScale = levelData.pieceScale;

            BoxCollider2D pieceCollider = pieceObj.GetComponent<BoxCollider2D>();
            pieceCollider.size = levelData.pieceColliderSize;

            PuzzlePiece piece = pieceObj.GetComponent<PuzzlePiece>();
            piece.correctIndex = levelData.pieceCorrectOrder[i];
            piece.spawnPosition = spawnPos;
            spawnedPieces.Add(pieceObj);

            string spriteName = levelData.pieceSprites[i];
            Sprite loadedSprite = Resources.Load<Sprite>($"Sprites/{spriteName}");

            if (loadedSprite != null)
            {
                pieceObj.GetComponent<SpriteRenderer>().sprite = loadedSprite;
            }
            else
            {
                Debug.LogError($"Sprite '{spriteName}' not found in Resources/Sprites/");
            }
        }
    }

    // Spawns the Target Squares depending on the json setting
    private void SpawnTargetSquares()
    {
        for (int i = 0; i < levelData.targetPositions.Length; i++)
        {
            GameObject target = Instantiate(targetSquarePrefab, levelData.targetPositions[i], Quaternion.identity);

            target.transform.localScale = levelData.targetScale;

            BoxCollider2D targetCollider = target.GetComponent<BoxCollider2D>();
            targetCollider.size = levelData.targetSquareColliderSize;

            TargetSquare square = target.GetComponent<TargetSquare>();
            square.index = i;

            allTargetSquaresList.Add(square);
        }
    }
    #endregion

    // Shuffle Method for List variables
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    public void CheckWinCondition()
    {
        foreach (TargetSquare square in allTargetSquaresList)
        {
            if (!square.IsOccupied || square.occupiedBy.correctIndex != square.index)
            {
                return;
            }
        }

        Debug.Log("Puzzle Complete!");
    }
}

[System.Serializable]
public class PuzzleLevelData
{
    [Header("Target Squares Configuration")]
    public Vector3 targetScale = Vector3.one;
    public Vector2 targetSquareColliderSize = Vector2.one;
    public Vector3[] targetPositions;

    [Header("Puzzle Pieces Configuration")]
    public int pieceCount;
    public string[] pieceSprites;
    public Vector3 pieceScale = Vector3.one;
    public Vector2 pieceColliderSize = Vector2.one;
    public Vector3[] pieceSpawnPoints;
    public int[] pieceCorrectOrder;
}