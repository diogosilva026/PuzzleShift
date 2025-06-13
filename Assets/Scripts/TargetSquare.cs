using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSquare : MonoBehaviour
{
    public int index; // To match the correctIndex of the matching puzzle piece
    public PuzzlePiece occupiedBy;

    public bool IsOccupied => occupiedBy != null;

    public void AssignPiece(PuzzlePiece piece)
    {
        occupiedBy = piece;
    }

    public void ClearPiece()
    {
        occupiedBy = null;
    }
}
