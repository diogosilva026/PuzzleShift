using System.Collections.Generic;
using UnityEngine;

public class PuzzlePieceSpawner : MonoBehaviour
{
    private PuzzleLevelData levelData; // Level data from json
    [SerializeField] private GameObject puzzlePiecePrefab;

    public List<PuzzlePiece> SpawnedPieces { get; private set; } = new();

    public void Initialize(PuzzleLevelData data)
    {
        levelData = data;
        SpawnPuzzlePieces();
    }

    // Spawn the grid of puzzle pieces based on piece count
    private void SpawnPuzzlePieces()
    {
        RectTransform container = GetComponent<RectTransform>();

        // Automatically calculate the correct order array by the puzzle size
        int[] correctOrder = new int[levelData.pieceCount];
        for (int i = 0; i < levelData.pieceCount; i++)
            correctOrder[i] = i;

        int columns = (int)Mathf.Sqrt(levelData.pieceCount);
        float spacing = 10f;

        // Adjust piece size depending on puzzle size
        float pieceSize = levelData.pieceCount switch
        {
            9 => 250f,
            16 => 175f,
            25 => 150f,
            _ => 200f
        };

        // Starting offset on the top left anchor position for the pieces' grid
        Vector2 startPos = new(-((columns - 1) * (pieceSize + spacing)) / 2f,
                                ((columns - 1) * (pieceSize + spacing)) / 2f);

        // Generate all possible positions for the grid
        List<Vector2> positions = new();
        for (int i = 0; i < levelData.pieceCount; i++)
        {
            int row = i / columns;
            int col = i % columns;
            Vector2 pos = startPos + new Vector2(col * (pieceSize + spacing), -row * (pieceSize + spacing));
            positions.Add(pos);
        }

        // Shuffle spawn positions
        for (int i = positions.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (positions[i], positions[j]) = (positions[j], positions[i]);
        }

        // Create all puzzle pieces
        for (int i = 0; i < levelData.pieceCount; i++)
        {
            GameObject puzzlePiece = Instantiate(puzzlePiecePrefab, container);
            puzzlePiece.name = $"PuzzlePiece{i}";

            RectTransform rect = puzzlePiece.GetComponent<RectTransform>();
            rect.anchoredPosition = positions[i];
            rect.sizeDelta = new Vector2(pieceSize, pieceSize);

            PuzzlePiece pieceScript = puzzlePiece.GetComponent<PuzzlePiece>();
            pieceScript.correctIndex = correctOrder[i];
            pieceScript.spawnPosition = rect.anchoredPosition;

            string spriteName = levelData.pieceSprites[i];
            Sprite loadedSprite = Resources.Load<Sprite>($"Sprites/{spriteName}");
            if (loadedSprite != null)
                pieceScript.spriteImage.sprite = loadedSprite;
            else
                Debug.LogError($"Sprite '{spriteName}' not found in Resources/Sprites/");

            SpawnedPieces.Add(pieceScript);
        }
    }
}
