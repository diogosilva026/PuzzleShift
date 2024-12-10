using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector2 mousePosition;
    public static bool isDragging;

    void Start()
    {
        isDragging = false;
    }

    void Update()
    {
        if (isDragging == true)
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
            mousePosition = Input.mousePosition;
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            isDragging = true;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }
}
