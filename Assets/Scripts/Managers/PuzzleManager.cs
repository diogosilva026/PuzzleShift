using UnityEngine;
using UnityEngine.SceneManagement;

// This script is responsible for loading level data, spawning puzzle pieces and target squares, and checking for puzzle completion
public class PuzzleManager : MonoBehaviour
{
    #region VARIABLES
    private PuzzleLevelData levelData;
    public PuzzlePiece SelectedPiece { get; private set; }
    private TargetSquareSpawner targetSpawner;
    private PuzzlePieceSpawner pieceSpawner;
    [SerializeField] private GameObject winScreen;
    #endregion

    // Get reference from the spawner scripts automatically
    private void Awake()
    {
        if (targetSpawner == null)
            targetSpawner = FindObjectOfType<TargetSquareSpawner>();

        if (pieceSpawner == null)
            pieceSpawner = FindObjectOfType<PuzzlePieceSpawner>();
    }

    private void Start()
    {
        LoadLevelData();
        SetupLevel();
    }

    // Loads the puzzle/level data from a json file named after the scene
    private void LoadLevelData()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        TextAsset jsonFile = Resources.Load<TextAsset>($"PuzzleConfigs/{sceneName}");

        if (jsonFile != null)
            levelData = JsonUtility.FromJson<PuzzleLevelData>(jsonFile.text);
        else
            Debug.LogError($"JSON file for scene {sceneName} not found!");
    }

    private void SetupLevel()
    {
        targetSpawner.Initialize(levelData);
        pieceSpawner.Initialize(levelData);

        TimerManager.Instance.StartTimer();
        winScreen.SetActive(false);
    }

    #region PIECE SELECTION STUFF
    public void SelectPiece(PuzzlePiece piece)
    {
        // Deselect if clicking the same piece
        if (SelectedPiece == piece)
        {
            piece.SetSelected(false);
            SelectedPiece = null;
            return;
        }

        // Deselect previously selected piece
        if (SelectedPiece != null)
        {
            SelectedPiece.SetSelected(false);
        }

        // Select new
        SelectedPiece = piece;
        SelectedPiece.SetSelected(true);
    }

    public void ClearSelectedPiece()
    {
        SelectedPiece = null;
    }

    public void DeselectPiece()
    {
        if (SelectedPiece != null)
        {
            SelectedPiece.SetSelected(false);
            SelectedPiece = null;
        }
    }
    #endregion

    // Checks if all puzzle pieces are correctly placed
    public void CheckWinCondition()
    {
        foreach (TargetSquare square in targetSpawner.SpawnedSquares)
        {
            if (!square.IsOccupied || square.occupiedBy.correctIndex != square.index)
                return;
        }

        PuzzleComplete();
    }

    // Gets called whenever a puzzle is completed
    private void PuzzleComplete()
    {
        PauseManager pauseManager = FindObjectOfType<PauseManager>();
        Destroy(pauseManager);

        winScreen.SetActive(true);
        TimerManager.Instance.EndTimer();
        AudioManager.Instance.PlaySFX("Applause");
    }
}