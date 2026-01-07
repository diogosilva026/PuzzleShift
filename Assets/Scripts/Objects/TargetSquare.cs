using UnityEngine;
using UnityEngine.EventSystems;

public class TargetSquare : MonoBehaviour, IPointerClickHandler
{
    public int index; // To match the correctIndex of the matching puzzle piece
    public PuzzlePiece occupiedBy; // Piece currently placed in this square

    public bool IsOccupied => occupiedBy != null;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Get PuzzleManager to access currently selected piece
        PuzzleManager puzzleManager = FindFirstObjectByType<PuzzleManager>();
        PuzzlePiece selected = puzzleManager.SelectedPiece;

        if (selected == null)
            return;

        PuzzlePiece other = occupiedBy; // Piece currently sitting in this square
        TargetSquare previousSquare = selected.currentTargetSquare; // Selected piece's previous location

        if (other == null)
        {
            // If the square is empty, place selected piece
            if (previousSquare != null)
                previousSquare.occupiedBy = null; // Clear old square

            PlacePiece(selected);
        }
        else
        {
            // If the square already has a piece, perform a swap
            if (previousSquare != null)
            {
                // Move the piece currently here back to the selected's previous square
                previousSquare.occupiedBy = other;
                MovePieceWithAnimation(other, ((RectTransform)previousSquare.transform).anchoredPosition, previousSquare.transform.parent);
                other.currentTargetSquare = previousSquare;
            }
            else
            {
                // If the selected piece came from spawn, move the "other" piece back to spawn
                other.MoveToSpawn();
            }

            // Place the selected piece into this square
            PlacePiece(selected);
        }

        puzzleManager.DeselectPiece();
        puzzleManager.CheckWinCondition();
    }

    // Place the puzzle piece on the current square
    private void PlacePiece(PuzzlePiece piece)
    {
        piece.currentTargetSquare = this;
        occupiedBy = piece;

        MovePieceWithAnimation(piece, ((RectTransform)transform).anchoredPosition, transform.parent);
    }

    // Animate puzzle piece movement withour breaking the UI anchoring
    public void MovePieceWithAnimation(PuzzlePiece piece, Vector2 targetAnchoredPos, Transform newParent)
    {
        // Save current world position before reparenting
        Vector3 worldPos = piece.rectTransform.position;

        // Reparent
        piece.rectTransform.SetParent(newParent, false);

        // Restore world position
        piece.rectTransform.position = worldPos;

        // Animate to target position
        piece.MoveTo(targetAnchoredPos);
    }
}
