using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzlePieceController : MonoBehaviour
{
    public GameObject targetPlace; // Correct position for this piece
    public List<GameObject> gameObjectsList; // List of all valid targets
    public GameObject winScreen;

    private Vector2 mousePosition;
    private bool isDragging = false;
    private bool isPlacedCorrectly = false; // Tracks if this piece is placed correctly

    private static int placedPieces = 0; // Tracks how many pieces are placed correctly
    private static int totalPieces; // Total number of puzzle pieces

    private static HashSet<GameObject> occupiedPositions = new HashSet<GameObject>(); // Tracks occupied positions

    private GameObject currentSnappedPosition = null; // Tracks the current position where this piece is snapped
    private GameObject pendingSnapPosition = null; // Tracks the position to snap to on mouse release

    void Start()
    {
        if (totalPieces == 0) // Initialize total pieces once
            totalPieces = gameObjectsList.Count;
    }

    void Update()
    {
        if (isDragging)
        {
            mousePosition = Input.mousePosition;
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = worldPosition;
        }
    }

    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition;
        isDragging = true;

        // Free up the position when picking up the piece
        if (currentSnappedPosition != null)
        {
            occupiedPositions.Remove(currentSnappedPosition);
            currentSnappedPosition = null;
            if (isPlacedCorrectly)
            {
                isPlacedCorrectly = false;
                placedPieces--;
            }
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;

        if (pendingSnapPosition != null && !occupiedPositions.Contains(pendingSnapPosition))
        {
            // Snap to the position if it's valid and not occupied
            transform.position = pendingSnapPosition.transform.position;
            currentSnappedPosition = pendingSnapPosition;
            occupiedPositions.Add(pendingSnapPosition);

            if (pendingSnapPosition == targetPlace)
            {
                isPlacedCorrectly = true;
                placedPieces++;
                CheckWinCondition();
            }
        }

        pendingSnapPosition = null; // Clear pending snap
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObjectsList.Contains(collision.gameObject) && !occupiedPositions.Contains(collision.gameObject))
        {
            // Set the pending snap position
            pendingSnapPosition = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (pendingSnapPosition == collision.gameObject)
        {
            // Clear pending snap if the mouse leaves the collider
            pendingSnapPosition = null;
        }
    }

    private void CheckWinCondition()
    {
        if (placedPieces == totalPieces)
        {
            winScreen.SetActive(true);
        }
    }
}
