using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePositions : MonoBehaviour
{
    public int patternID;
    public PuzzleManager puzzleManager;
    public GameObject assignedObject;

    void Update()
    {
        if (assignedObject != null)
        {
            assignedObject.transform.position = gameObject.transform.position;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Piece"))
        {
            // Allow reassignment only if the piece is not already assigned to this position
            if (assignedObject == null)
            {
                // Check if it's a possible object
                for (int i = 0; i < puzzleManager.possibleObjects.Length; i++)
                {
                    if (collision.gameObject == puzzleManager.possibleObjects[i])
                    {
                        assignedObject = collision.gameObject;
                        //DragAndDrop.isDragging = false;
                    }
                }
            }
        }
    }
}
