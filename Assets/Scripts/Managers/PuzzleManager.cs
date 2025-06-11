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
    [SerializeField] private List<GameObject> spawnedPieces = new();

    private void Start()
    {
        LoadLevelData();
        SpawnPuzzlePieces();
    }

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

        for (int i = 0; i < levelData.pieceCount; i++)
        {
            Vector3 spawnPos = levelData.pieceSpawnPoints[i % levelData.pieceSpawnPoints.Length];
            GameObject pieceObj = Instantiate(puzzlePiecePrefab, spawnPos, Quaternion.identity);
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
}

[System.Serializable]
public class PuzzleLevelData
{
    [Header("Scene Configuration")]
    public string sceneName;
    public int targetSquareCount;

    [Header("Piece Configuration")]
    public int pieceCount;
    public string[] pieceSprites;
    public Vector3[] pieceSpawnPoints;
    public Vector3[] targetPositions;
    public int[] pieceCorrectOrder;
}