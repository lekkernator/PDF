using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject label;
    private TextMeshProUGUI characterText;
    private Image outlineImage;
    public GameObject selectionTile;
    private Image selectionToBeColored; 
    public bool isSelected = false;
    private CharacterSelection selection;
    public string sceneName;

    private void Start()
    {

        selection = selectionTile.GetComponent<CharacterSelection>();
        selectionToBeColored = selectionTile.GetComponent<Image>();
        label.SetActive(true);
        characterText = label.GetComponent<TextMeshProUGUI>();
        characterText.enabled = false;
        outlineImage = GetComponent<Image>();
        // Initially, disable the character text.
        characterText.gameObject.SetActive(false);
    }
    void Update()
    {
        label.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        characterText.enabled = true;
        selectionToBeColored.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Change the outline color back to black when the mouse exits.
        if (!isSelected)
        {
            characterText.enabled = false;
            selectionToBeColored.color = Color.black;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selection.isSelected = !isSelected;
        // Toggle the text visibility and change the outline color to red.
        isSelected = !isSelected;
        //selectionToBeColored.color = isSelected ? Color.red : Color.black;
    }
    public void SelectCharacter(bool select)
    {
        if(!select)
        {
            selectionToBeColored.color = Color.black;
            characterText.enabled = false;
        }
        selection.isSelected = select;
        isSelected = select;
    }
}
