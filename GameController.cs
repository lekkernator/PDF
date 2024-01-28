using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject pauseMenuPrefab; // Assign the pause menu prefab in the Unity editor
    private GameObject currentPauseMenu;
    private SpriteRenderer pause;
    private bool isPaused = false;
    private bool gameOver = false;

    private void Start()
    {
        pause = GetComponent<SpriteRenderer>();
    }
    void Update()
    {

        // Check for the escape key press
        if (Input.GetKeyDown(KeyCode.Escape) && (Time.timeScale == 1 || isPaused))
        {
            TogglePause();
        }
            if (Time.timeScale != 1 && !isPaused && IsKeyboardKeyPressed())
        {
            TransitionToNextScene();
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused; // Toggle the paused state

        if (isPaused)
        {
            Time.timeScale = 0f; // Pause the game
            currentPauseMenu = Instantiate(pauseMenuPrefab, transform.position, Quaternion.identity); // Spawn the pause menu
            pause.enabled = true;
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
            if (currentPauseMenu != null)
            {
                Destroy(currentPauseMenu); // Despawn the pause menu
            }
        }
    }
    private void TransitionToNextScene()
    {
        SceneManager.LoadScene("GameOverMenu");
    }

    private bool IsKeyboardKeyPressed()
    {
        // Check for any key press, excluding mouse buttons
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Mouse buttons are usually the first few buttons
            if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
            {
                return true;
            }
        }
        return false;
    }
}