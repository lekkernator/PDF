using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpriteButton : MonoBehaviour
{
    public string sceneToLoad;
    public string transitionType;
    public bool playTransitionSound;
    public Animator fadeAnimator; // Assign the Animator component of your UI Panel
    private AudioSource audioSource; // Assign the AudioSource component
    public AudioClip transitionSound; // Assign this in the Unity editor
    private CharacterSelectionManager[] characterSelectionManager;


    void Start()
    {
        Time.timeScale = 1.0f;
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component.
        characterSelectionManager = FindObjectsOfType<CharacterSelectionManager>();
    }
    private IEnumerator OnMouseDown()
    {

        fadeAnimator.SetTrigger(transitionType);
        // Start fading out the audio
        if (!playTransitionSound)
        {
            //yield return new WaitForSeconds(1f);
            StartCoroutine(FadeOutAudio(1f)); // Fade out over 1 second
            yield return new WaitForSeconds(1f);
        }
        else 
        {
            // Wait for the fade-out animation to finish (0.5 second)
            yield return new WaitForSeconds(0.5f);
        
            audioSource.PlayOneShot(transitionSound); // Play the transition sound
            // Wait for the fade-out animation to finish (0.5 second)
            yield return new WaitForSeconds(0.5f);
        }
        if (sceneToLoad == "HundredAcreWoods")
        {
            var scene = characterSelectionManager[characterSelectionManager.Length-1].getCurrentSceneNameDeath();
            Debug.Log(scene);
            Debug.Log(scene);
            SceneManager.LoadScene(scene);
        }
        else
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
    private IEnumerator FadeOutAudio(float fadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            // Decrease volume linearly
            audioSource.volume -= startVolume * .01f / fadeTime;
            yield return new WaitForSeconds(0.01f);
        }

        audioSource.volume = 0; // Ensure the volume is set to 0 at the end
        yield return null;
    }
}
