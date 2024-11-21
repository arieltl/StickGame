using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Reference to the pause menu UI panel
    public GameManager gameMangager;

    private bool isPaused = false;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);  // Ensure the pause menu persists between scenes
        pauseMenuUI.SetActive(false); // Hide the pause menu at the start of the game
    }

    public void onPause(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        // Hide the pause menu and resume the game
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ExitToMainMenu()
    {
        pauseMenuUI.SetActive(false);
        gameMangager.isGameRunning = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");  // Replace "MainMenu" with your actual main menu scene name
    }
}
