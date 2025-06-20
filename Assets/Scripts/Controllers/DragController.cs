using System.Collections.Generic;
using UnityEngine;

// This script handles dragging and dropping of puzzle pieces onto target squares
public class DragController : MonoBehaviour
{
    private bool isDragging = false;
    private PuzzlePiece lastDragged;
    private Vector3 originalPosition;
    private TargetSquare snappedSquare;
    public List<TargetSquare> validTargets;

    private void Awake()
    {
        DragController[] controllers = FindObjectsOfType<DragController>();
        if (controllers.Length > 1)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Vector3 mouseWorldPos = GetMouseWorldPosition();

        HandleMouseDown(mouseWorldPos);
        HandleDragging(mouseWorldPos);
        HandleMouseUp();
    }

    // Converts screen mouse position to world space position
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        worldPos.z = 0f;
        return worldPos;
    }

    // Initiates dragging if a puzzle piece is clicked
    private void HandleMouseDown(Vector3 mouseWorldPos)
    {
        if (!Input.GetMouseButtonDown(0)) return;

        // Unity often fails to hit 2D colliders unless you filter by layer explicitly
        int puzzleLayerMask = LayerMask.GetMask("PuzzlePiece");
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, Mathf.Infinity, puzzleLayerMask);

        if (hit.collider != null)
        {
            PuzzlePiece draggable = hit.transform.GetComponent<PuzzlePiece>();
            if (draggable != null)
            {
                BeginDrag(draggable);
            }
        }
    }

    // Updates piece position while dragging
    private void HandleDragging(Vector3 mouseWorldPos)
    {
        if (isDragging && lastDragged != null)
        {
            DragTo(mouseWorldPos);
        }
    }

    // Handles dropping the piece on mouse release
    private void HandleMouseUp()
    {
        if (isDragging && Input.GetMouseButtonUp(0))
        {
            Drop();
        }
    }

    // Starts the drag operation for a selected piece
    private void BeginDrag(PuzzlePiece piece)
    {
        lastDragged = piece;

        // If the piece is already snapped to a square, remember that as its original position
        if (snappedSquare != null && piece.transform.position == snappedSquare.transform.position)
        {
            originalPosition = snappedSquare.transform.position;
        }
        else
        {
            originalPosition = piece.transform.position;
        }

        isDragging = true;
    }

    // Moves the piece to follow the mouse cursor
    private void DragTo(Vector3 position)
    {
        lastDragged.transform.position = (Vector2)position;
    }

    // Drops the piece into a valid target square or resets its position
    private void Drop()
    {
        isDragging = false;

        if (lastDragged != null && validTargets != null)
        {
            // Clear previous square reference if any
            if (lastDragged.currentTargetSquare != null)
            {
                lastDragged.currentTargetSquare.ClearPiece();
                lastDragged.currentTargetSquare = null;
            }

            float snapRange = 0.5f; // Max distance allowed for snapping
            TargetSquare closest = null;
            float minDistance = Mathf.Infinity;

            // Find the closest valid target square within snap range
            foreach (TargetSquare target in validTargets)
            {
                float distance = Vector2.Distance(lastDragged.transform.position, target.transform.position);
                if (distance < minDistance && distance <= snapRange)
                {
                    minDistance = distance;
                    closest = target;
                }
            }

            // If a valid target square is found and is unoccupied, snap the piece
            if (closest != null && !closest.IsOccupied)
            {
                // Snap to target square and mark it as occupied
                lastDragged.transform.position = closest.transform.position;
                closest.AssignPiece(lastDragged);
                lastDragged.currentTargetSquare = closest;
                snappedSquare = closest;
            }
            else
            {
                // Return to spawn position
                lastDragged.transform.position = lastDragged.spawnPosition;
                snappedSquare = null;
            }
        }

        // Check if the puzzle is complete after dropping
        PuzzleManager puzzleManager = FindObjectOfType<PuzzleManager>();
        puzzleManager.CheckWinCondition();

        // Clear reference to the dragged piece
        lastDragged = null;
    }
}
