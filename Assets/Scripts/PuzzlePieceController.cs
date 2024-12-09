using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

public class PuzzlePieceController : MonoBehaviour
{
    public GameObject targetPlace;
    public List<GameObject> gameObjectsList;

    private Vector2 mousePosition;
    private bool isDragging = false;
    public int score = 0;

    void Start()
    {
        score = 0;
    }

    void Update()
    {
        if (isDragging == true)
        {
            mousePosition = Input.mousePosition;
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = worldPosition;
        }
        if (score == 9)
        {
            SceneManager.LoadScene("Win");
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (GameObject gameObjectL in gameObjectsList)
        {
            if (collision.gameObject == gameObjectL)
            {
                gameObject.transform.position = gameObjectL.transform.position;
                isDragging = false;
            }
        }

        if (collision.gameObject == targetPlace)
        {
            gameObject.transform.position = targetPlace.transform.position;
            AddScore();
            //Destroy(collision.gameObject.GetComponent<Collider2D>());
            isDragging = false;
        }
    }
    public void AddScore()
    {
        score++;
    }
}
