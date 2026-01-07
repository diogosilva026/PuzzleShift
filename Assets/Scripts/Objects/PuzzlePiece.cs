using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour, IPointerClickHandler
{
    #region VARIABLES
    public int correctIndex;
    public TargetSquare currentTargetSquare;
    public Vector2 spawnPosition;

    [SerializeField] private Image outline;
    public Image spriteImage;

    [HideInInspector] public RectTransform rectTransform;
    #endregion

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PuzzleManager puzzleManager = FindFirstObjectByType<PuzzleManager>();

        if (puzzleManager.SelectedPiece == this)
        {
            // Deselect if clicking the same piece
            SetSelected(false);
            puzzleManager.ClearSelectedPiece();
            return;
        }

        if (currentTargetSquare != null && puzzleManager.SelectedPiece != null)
        {
            // If this piece is placed and another piece is selected let its square handle swapping logic
            currentTargetSquare.OnPointerClick(eventData);
            return;
        }

        puzzleManager.SelectPiece(this);
    }

    public void MoveToSpawn()
    {
        // Find the piece container and move the piece there
        PuzzlePieceSpawner spawner = FindFirstObjectByType<PuzzlePieceSpawner>();
        TargetSquare targetSquare = FindFirstObjectByType<TargetSquare>();
        if (spawner != null)
        {
            RectTransform pieceContainer = spawner.GetComponent<RectTransform>();
            targetSquare.MovePieceWithAnimation(this, spawnPosition, pieceContainer);
        }

        // Clear square reference
        currentTargetSquare = null;
    }

    // Set selected piece
    public void SetSelected(bool selected)
    {
        outline.enabled = selected;
    }

    public void MoveTo(Vector2 targetPos, float duration = 0.15f)
    {
        StopAllCoroutines();
        StartCoroutine(MoveAnimation(targetPos, duration));
    }

    private IEnumerator MoveAnimation(Vector2 targetPos, float duration)
    {
        Vector2 startPos = rectTransform.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float ease = Mathf.SmoothStep(0, 1, elapsedTime / duration); // Smooth easing

            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, ease);
            yield return null;
        }

        // Final snap to ensure accuracy
        rectTransform.anchoredPosition = targetPos;
    }
}
