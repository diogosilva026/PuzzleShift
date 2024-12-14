using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzlePieces : MonoBehaviour
{
    public GameObject targetPlace; // Correct position for this piece
    public List<GameObject> gameObjectsList; // List of all valid targets

    private Vector2 mousePosition;
    private bool isDragging = false;
    private bool isPlacedCorrectly = false; // Tracks if this piece is placed correctly

    private static int placedPieces = 0; // Tracks how many pieces are placed correctly
    private static int totalPieces; // Total number of puzzle pieces

    private static HashSet<GameObject> occupiedPositions = new HashSet<GameObject>(); // Tracks occupied positions

    private GameObject currentSnappedPosition = null; // Tracks the current position where this piece is snapped

    void Start()
    {
        if (totalPieces == 0) // Initialize total pieces once
        {
            totalPieces = gameObjectsList.Count;
        }
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObjectsList.Contains(collision.gameObject) && !occupiedPositions.Contains(collision.gameObject))
        {
            // Snap to the position if it's not occupied
            transform.position = collision.gameObject.transform.position;
            isDragging = false;

            // Mark the position as occupied
            currentSnappedPosition = collision.gameObject;
            occupiedPositions.Add(collision.gameObject);

            // Check if it's the correct target place
            if (collision.gameObject == targetPlace)
            {
                isPlacedCorrectly = true;
                placedPieces++;
                CheckWinCondition();
            }
        }
    }

    private void CheckWinCondition()
    {
        if (placedPieces == totalPieces)
        {
            Debug.Log("All pieces placed correctly!");
            SceneManager.LoadScene("Win"); // Load win scene
        }
    }
}
