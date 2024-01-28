using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageTransition : MonoBehaviour, IPointerClickHandler
{
    //public GameObject MusicManager;
    private MusicManager[] musicManager;
    public GameObject SelectionManager;
    private CharacterSelectionManager selectionManager;
   // public Animator fadeAnimator; // Assign the Animator component of your UI Panel
    private AudioSource audioSource; // Assign the AudioSource component
    public AudioClip transitionSound; // Assign this in the Unity editor
    private bool sceneTransitionInProgress = false;

    private void Start()
    {
        //musicManager = MusicManager.GetComponent<MusicManager>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        selectionManager = SelectionManager.GetComponent<CharacterSelectionManager>();
        musicManager = FindObjectsOfType<MusicManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(dothings());
    }
    IEnumerator dothings()
    {
        if (selectionManager.getCurrentSceneTransition() != null && !sceneTransitionInProgress)
        {
            sceneTransitionInProgress = true;

            //fadeAnimator.SetTrigger("FadeOut");

            // Wait for the fade-out animation to finish (0.5 second)
            audioSource.PlayOneShot(transitionSound); // Play the transition sound

            // Wait for the fade-out animation to finish (0.5 second)
            yield return new WaitForSeconds(0.4f);
            foreach(var m in musicManager)
            {
                m.PauseMusic();
            }
            SceneManager.LoadScene(selectionManager.getCurrentSceneTransition());
        }
    }
}
