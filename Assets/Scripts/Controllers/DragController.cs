using System.Collections.Generic;
using UnityEngine;

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

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        worldPos.z = 0f;
        return worldPos;
    }

    private void HandleMouseDown(Vector3 mouseWorldPos)
    {
        if (!Input.GetMouseButtonDown(0)) return;

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

    private void HandleDragging(Vector3 mouseWorldPos)
    {
        if (isDragging && lastDragged != null)
        {
            DragTo(mouseWorldPos);
        }
    }

    private void HandleMouseUp()
    {
        if (isDragging && Input.GetMouseButtonUp(0))
        {
            Drop();
        }
    }

    private void BeginDrag(PuzzlePiece piece)
    {
        lastDragged = piece;

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

    private void DragTo(Vector3 position)
    {
        lastDragged.transform.position = new Vector2(position.x, position.y);
    }

    private void Drop()
    {
        isDragging = false;

        if (lastDragged != null && validTargets != null)
        {
            float snapRange = 0.5f;
            TargetSquare closest = null;
            float minDistance = Mathf.Infinity;

            foreach (TargetSquare target in validTargets)
            {
                float distance = Vector2.Distance(lastDragged.transform.position, target.transform.position);
                if (distance < minDistance && distance <= snapRange)
                {
                    minDistance = distance;
                    closest = target;
                }
            }

            if (closest != null)
            {
                lastDragged.transform.position = closest.transform.position;
                snappedSquare = closest;
            }
            else
            {
                lastDragged.transform.position = lastDragged.spawnPosition;
                snappedSquare = null;
            }
        }

        lastDragged = null;
    }
}
