using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script is responsible for loading level data, spawning puzzle pieces and target squares, and checking for puzzle completion
public class PuzzleManager : MonoBehaviour
{
    #region VARIABLES
    private PuzzleLevelData levelData; // Stores json configured level data
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject puzzlePiecePrefab;
    [SerializeField] private GameObject targetSquarePrefab;
    [SerializeField] private List<GameObject> allSpawnedPiecesList = new();
    [SerializeField] private List<TargetSquare> allTargetSquaresList = new();
    #endregion

    private void Start()
    {
        // Provide DragController with a reference to all target squares
        DragController dragController = FindObjectOfType<DragController>();
        if (dragController != null )
        {
            dragController.validTargets = allTargetSquaresList;
        }
        
        StartLevel();
    }

    private void StartLevel()
    {
        LoadLevelData();
        SpawnPuzzlePieces();
        SpawnTargetSquares();

        winScreen.SetActive(false);
        //TimerManager.Instance.StartTimer();
    }

    // Loads the chosen puzzle/level data from a json file named after the scene
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
    // Spawns the puzzle pieces using data from the json file
    private void SpawnPuzzlePieces()
    {
        // Creates a copy of the spawn points and shuffles them
        List<Vector3> shuffledSpawnPoints = new(levelData.pieceSpawnPoints);
        ShuffleList(shuffledSpawnPoints);

        for (int i = 0; i < levelData.pieceCount; i++)
        {
            Vector3 spawnPos = shuffledSpawnPoints[i];

            // Instantiate puzzle piece
            GameObject pieceObj = Instantiate(puzzlePiecePrefab, spawnPos, Quaternion.identity);
            pieceObj.layer = LayerMask.NameToLayer("PuzzlePiece");
            pieceObj.transform.localScale = levelData.pieceScale;

            // Set collider size
            BoxCollider2D pieceCollider = pieceObj.GetComponent<BoxCollider2D>();
            pieceCollider.size = levelData.pieceColliderSize;

            // Setup puzzle piece properties
            PuzzlePiece piece = pieceObj.GetComponent<PuzzlePiece>();
            piece.correctIndex = levelData.pieceCorrectOrder[i];
            piece.spawnPosition = spawnPos;

            allSpawnedPiecesList.Add(pieceObj);

            // Assign puzzle piece sprite
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

    // Spawns the target squares using data from the json file
    private void SpawnTargetSquares()
    {
        for (int i = 0; i < levelData.targetPositions.Length; i++)
        {
            // Instantiate target square
            GameObject targetObj = Instantiate(targetSquarePrefab, levelData.targetPositions[i], Quaternion.identity);
            targetObj.transform.localScale = levelData.targetScale;

            // Set collider size
            BoxCollider2D targetCollider = targetObj.GetComponent<BoxCollider2D>();
            targetCollider.size = levelData.targetSquareColliderSize;

            // Setup target square index
            TargetSquare square = targetObj.GetComponent<TargetSquare>();
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

    // Checks if all puzzle pieces are correctly placed
    public void CheckWinCondition()
    {
        foreach (TargetSquare square in allTargetSquaresList)
        {
            if (!square.IsOccupied || square.occupiedBy.correctIndex != square.index)
            {
                return;
            }
        }

        foreach (GameObject pieceObj in allSpawnedPiecesList)
        {
            Collider2D col = pieceObj.GetComponent<Collider2D>();
            col.enabled = false;
        }

        //TimerManager.Instance.EndTimer();
        winScreen.SetActive(true);
        Debug.Log("Puzzle Complete!");
    }
}

// Represents the level data loaded from the json file
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