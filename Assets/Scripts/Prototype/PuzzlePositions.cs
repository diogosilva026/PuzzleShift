using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePositions : MonoBehaviour
{
    public int patternID;
    public PuzzleManager puzzleManager;
    public GameObject assignedObject;

    void Start()
    {

    }

    void Update()
    {
        if (assignedObject != null)
        {
            assignedObject.transform.position = gameObject.transform.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Piece") && assignedObject == null)
        {
            // Check if it's a possible object
            for (int i = 0; i < puzzleManager.possibleObjects.Length; i++)
            {
                if (other.gameObject == puzzleManager.possibleObjects[i])
                {
                    assignedObject = other.gameObject;
                    DragAndDrop.isDragging = false;
                }
            }
        }
    }
}
