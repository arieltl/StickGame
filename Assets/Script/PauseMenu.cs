using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Reference to the pause menu UI panel

    private bool isPaused = false;

    void Start()
    {
        // Ensure the pause menu is hidden when the game starts
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        // Toggle pause menu when Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
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
        // Show the pause menu and pause the game
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void RestartLevel()
    {
        // Reload the current scene and resume the game
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitToMainMenu()
    {
        // Resume time and load the main menu scene
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");  // Replace "MainMenu" with your actual main menu scene name
    }
}
