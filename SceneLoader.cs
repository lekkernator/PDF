using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    CharacterSelectionManager characterSelectionManager;
    private void Start()
    {
        characterSelectionManager = FindObjectOfType<CharacterSelectionManager>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Credits");
        }
    }
    public void LoadStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void LoadHundredAcreWoods()
    {
        var scene = characterSelectionManager.getCurrentSceneNameDeath();
        Debug.Log(scene);
        Destroy(characterSelectionManager);
        Debug.Log(scene);
        SceneManager.LoadScene(scene);
    }
}
