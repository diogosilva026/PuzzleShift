using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public int correctIndex; // The correct index that this puzzle piece should match with a TargetSquare
    public Vector3 spawnPosition; // Original position where the puzzle piece spawns
    public TargetSquare currentTargetSquare; // The target square this piece is currently occupying
}
