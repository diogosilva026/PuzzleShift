using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private bool isGamePaused = false;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void Start()
    {
        pauseScreen.SetActive(false);
    }

    private void OnEnable()
    {
        playerInput.UI.Pause.performed += OnPauseInput;
        playerInput.UI.Enable();
    }

    private void OnDisable()
    {
        playerInput.UI.Pause.performed -= OnPauseInput;
        playerInput.UI.Disable();
    }

    private void OnPauseInput(InputAction.CallbackContext context)
    {
        if (isGamePaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        TimerManager.Instance.PauseTimer();
        pauseScreen.SetActive(true);
        isGamePaused = true;
    }

    public void ResumeGame()
    {
        TimerManager.Instance.ResumeTimer();
        pauseScreen.SetActive(false);
        isGamePaused = false;
    }

    public void SetPauseInputEnabled(bool enabled)
    {
        if (enabled)
            playerInput.UI.Enable();
        else
        {
            playerInput.UI.Disable();
            pauseScreen.SetActive(false);
        }
    }
}
