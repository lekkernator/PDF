using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    public GameObject stage1;
    public GameObject stage2;
    private CharacterSelection character1;
    private CharacterSelection character2;
    private CharacterSelection previousSelectedCharacter;
    private string sceneName = "";
    private void Awake()
    {
        // Ensure this GameObject persists across scenes
        DontDestroyOnLoad(gameObject);
        character1 = stage1.GetComponent<CharacterSelection>();
        character2 = stage2.GetComponent<CharacterSelection>();
    }
    
    void Update()
    {
        // Check if character1 is selected and previously selected character is not character1
        if (character1.isSelected && previousSelectedCharacter != character1)
        {
            HandleSelection(character1, character2);
        }
        // Check if character2 is selected and previously selected character is not character2
        else if (character2.isSelected && previousSelectedCharacter != character2)
        {
            HandleSelection(character2, character1);
        }
    }

    private void HandleSelection(CharacterSelection selectedCharacter, CharacterSelection otherCharacter)
    {
        // Deselect the other character
        otherCharacter.SelectCharacter(false);

        // Update the previous selected character
        previousSelectedCharacter = selectedCharacter;
    }
    public string getCurrentSceneTransition()
    {
        sceneName = previousSelectedCharacter.sceneName;
        Debug.Log(sceneName);
        return sceneName;
    }
    public string getCurrentSceneNameDeath()
    {
        Debug.Log(sceneName);
        return sceneName;
    }
}
