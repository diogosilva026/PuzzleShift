using System.Collections.Generic;
using UnityEngine;

public class TargetSquareSpawner : MonoBehaviour
{
    private PuzzleLevelData levelData; // Level data from json
    [SerializeField] private GameObject targetSquarePrefab;

    public List<TargetSquare> SpawnedSquares { get; private set; } = new();

    public void Initialize(PuzzleLevelData data)
    {
        levelData = data;
        SpawnTargetSquares();
    }

    // Spawn the grid of target squares based on piece count
    private void SpawnTargetSquares()
    {
        RectTransform container = GetComponent<RectTransform>();

        int columns = (int)Mathf.Sqrt(levelData.pieceCount);
        float spacing = 10f;

        // Adjust square size depending on puzzle size
        float squareSize = levelData.pieceCount switch
        {
            9 => 250f,
            16 => 175f,
            25 => 150f,
            _ => 200f
        };

        // Starting offset on the top left anchor position for the square grid
        Vector2 startPos = new(-((columns - 1) * (squareSize + spacing)) / 2f,
                                ((columns - 1) * (squareSize + spacing)) / 2f);

        // Create all squares
        for (int i = 0; i < levelData.pieceCount; i++)
        {
            GameObject square = Instantiate(targetSquarePrefab, container);
            square.name = $"TargetSquare{i}";

            int row = i / columns;
            int col = i % columns;

            // Position in the grid
            RectTransform rect = square.GetComponent<RectTransform>();
            rect.anchoredPosition = startPos + new Vector2(col * (squareSize + spacing), -row * (squareSize + spacing));
            rect.sizeDelta = new Vector2(squareSize, squareSize);

            // Assign index and register on the list
            TargetSquare squareScript = square.GetComponent<TargetSquare>();
            squareScript.index = i;
            SpawnedSquares.Add(squareScript);
        }
    }
}
