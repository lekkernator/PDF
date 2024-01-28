using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class CreditsManager : MonoBehaviour
{


    void Update()
    {
        if (IsKeyboardKeyPressed())
        {
            SceneManager.LoadScene("GameOverMenu");
        }
    }

    private bool IsKeyboardKeyPressed()
    {
        // Check for any key press, excluding mouse buttons
        if (Input.anyKeyDown)
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