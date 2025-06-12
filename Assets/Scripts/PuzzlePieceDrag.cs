using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PuzzlePiece))]
public class PuzzlePieceDrag : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private Vector3 originalPosition;
    private Camera mainCamera;
    private PuzzlePiece piece;

    private void Start()
    {
        mainCamera = Camera.main;
        piece = GetComponent<PuzzlePiece>();
        originalPosition = transform.position;
    }

    private void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        if (isDragging)
            transform.position = GetMouseWorldPosition() + offset;
    }

    private void OnMouseUp()
    {
        isDragging = false;

        //Collider2D hit = Physics2D.OverlapPoint(transform.position);
        Collider2D hit = Physics2D.OverlapPoint(GetMouseWorldPosition());
        if (hit != null && hit.TryGetComponent(out TargetSquare square))
        {
            transform.position = square.transform.position;

            if (square.index == piece.correctIndex)
            {
                piece.isPlacedCorrectly = true;
            }
            else
            {
                piece.isPlacedCorrectly = false;
            }
        }
        else
        {
            transform.position = originalPosition;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z; // Distance from camera
        return mainCamera.ScreenToWorldPoint(mousePos);
    }
}
