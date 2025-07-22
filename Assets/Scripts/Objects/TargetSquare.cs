using UnityEngine;

public class TargetSquare : MonoBehaviour
{
    public int index; // To match the correctIndex of the matching puzzle piece
    public PuzzlePiece occupiedBy; // Reference to the piece currently snapped into this target square

    public bool IsOccupied => occupiedBy != null;

    // Assigns a puzzle piece to this square
    public void AssignPiece(PuzzlePiece piece)
    {
        occupiedBy = piece;
    }

    // Clears the reference to the assigned puzzle piece
    public void ClearPiece()
    {
        occupiedBy = null;
    }
}
