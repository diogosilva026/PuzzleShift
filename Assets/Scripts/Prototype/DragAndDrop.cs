using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector2 mousePosition;
    private bool isDragging;
    public PuzzleManager puzzleManager;

    void Start()
    {
        isDragging = false;
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
        if (gameObject.tag == "Piece")
        {
            isDragging = true;

            mousePosition = Input.mousePosition;
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }
}
