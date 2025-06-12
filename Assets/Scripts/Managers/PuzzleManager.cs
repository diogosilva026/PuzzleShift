using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    private PuzzleLevelData levelData;
    [SerializeField] private GameObject puzzlePiecePrefab;
    [SerializeField] private GameObject targetSquarePrefab;
    [SerializeField] private List<GameObject> spawnedPieces = new();

    private void Start()
    {
        LoadLevelData();
        SpawnTargetSquares();
        SpawnPuzzlePieces();
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

    private void SpawnPuzzlePieces()
    {
        if (levelData == null)
        {
            Debug.LogError("Level data not loaded.");
            return;
        }

        // Creates a copy of the spawn points and shuffles them
        List<Vector3> shuffledSpawnPoints = new(levelData.pieceSpawnPoints);
        ShuffleList(shuffledSpawnPoints);

        for (int i = 0; i < levelData.pieceCount; i++)
        {
            Vector3 spawnPos = shuffledSpawnPoints[i];

            GameObject pieceObj = Instantiate(puzzlePiecePrefab, spawnPos, Quaternion.identity);

            pieceObj.transform.localScale = levelData.pieceScale;

            BoxCollider2D col = pieceObj.GetComponent<BoxCollider2D>();
            col.size = new Vector2(2 / pieceObj.transform.localScale.x, 2 / pieceObj.transform.localScale.y);

            PuzzlePiece piece = pieceObj.GetComponent<PuzzlePiece>();
            piece.correctIndex = levelData.pieceCorrectOrder[i];
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

    private void SpawnTargetSquares()
    {
        for (int i = 0; i < levelData.targetPositions.Length; i++)
        {
            GameObject target = Instantiate(targetSquarePrefab, levelData.targetPositions[i], Quaternion.identity);
            target.transform.localScale = levelData.targetScale;
            TargetSquare square = target.GetComponent<TargetSquare>();
            square.index = i;
        }
    }

    // Shuffle Method for List variables
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    public void CheckIfPuzzleIsComplete()
    {
        foreach (GameObject pieceObj in spawnedPieces)
        {
            PuzzlePiece piece = pieceObj.GetComponent<PuzzlePiece>();
            if (!piece.isPlacedCorrectly)
                return;
        }

        Debug.Log("Puzzle Complete!");
        // Show win UI, play sound, etc.
    }
}

[System.Serializable]
public class PuzzleLevelData
{
    [Header("Target Squares Configuration")]
    public Vector3 targetScale = Vector3.one;
    public Vector3[] targetPositions;

    [Header("Puzzle Pieces Configuration")]
    public int pieceCount;
    public string[] pieceSprites;
    public Vector3 pieceScale = Vector3.one;
    public Vector3[] pieceSpawnPoints;
    public int[] pieceCorrectOrder;
}