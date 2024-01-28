using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public GameObject MusicManager;
    private MusicManager musicManager;
    public Animator fadeAnimator; // Assign the Animator component of your UI Panel
    private AudioSource audioSource; // Assign the AudioSource component
    public AudioClip transitionSound; // Assign this in the Unity editor
    private bool sceneTransitionInProgress = false;

    void Start()
    {
        musicManager = MusicManager.GetComponent<MusicManager>();
        musicManager.PlayMusic();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component.
    }

    void Update()
    {
        if (!sceneTransitionInProgress && IsKeyboardKeyPressed())
        {
            StartCoroutine(TransitionToNextScene());
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

    IEnumerator TransitionToNextScene()
    {
        sceneTransitionInProgress = true;

        fadeAnimator.SetTrigger("FadeOut");

        // Wait for the fade-out animation to finish (0.5 second)
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(transitionSound); // Play the transition sound

        // Wait for the fade-out animation to finish (0.5 second)
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("StageSelection");
    }
}

