using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzlePieceController : MonoBehaviour
{
    public GameObject targetPlace; // Correct position for this puzzle piece
    public List<GameObject> gameObjectsList; // List of all valid targets
    public GameObject winScreen; // WinScreen Canvas
    private AudioManager audioManager; // Reference to AudioManager

    private Vector2 mousePosition;
    private bool isDragging = false;
    private bool isPlacedCorrectly = false; // Tracks if this puzzle piece is placed correctly

    private static int placedPieces = 0; // Tracks how many puzzle pieces are placed correctly
    private static int totalPieces; // Total number of puzzle pieces

    private static HashSet<GameObject> occupiedPositions = new HashSet<GameObject>(); // Tracks occupied positions

    private GameObject currentSnappedPosition = null; // Tracks the current position where this puzzle piece is snapped
    private GameObject pendingSnapPosition = null; // Tracks the position to snap to on mouse release

    private bool gameCompleted = false; // Tracks if the win condition is completed

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();

        // Reset static variables for a new scene
        placedPieces = 0;
        totalPieces = gameObjectsList.Count;
        occupiedPositions.Clear();
    }

    void Update()
    {
        if (gameCompleted) return; // Prevent updates if the win condition is completed

        if (isDragging)
        {
            mousePosition = Input.mousePosition;
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = worldPosition;
        }
    }

    private void OnMouseDown()
    {
        if (gameCompleted) return; // Prevent interaction if the win condition is completed

        mousePosition = Input.mousePosition;
        isDragging = true;

        // Free up the position when picking up the puzzle piece
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
        if (gameCompleted) return; // Prevent interaction if the win condition is completed

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
        if (gameCompleted) return; // Prevent interaction if the win condition is completed

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
            gameCompleted = true; // Prevent further interaction
            if (audioManager != null)
            {
                audioManager.PlayApplause(); // Play the applause sound
            }
        } 
    }
}
