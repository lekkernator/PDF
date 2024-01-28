using UnityEngine;
using TMPro;
using System; // Make sure to include the TextMeshPro namespace

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextColorChanger : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    public Color startColor = Color.black;
    public Color endColor = new Color(0.6f, 0f, 0f); // Deep crimson color
    public float maxPercentage = 200f;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        string text = textMesh.text;
        float percentage = ExtractPercentage(text);
        UpdateTextColor(percentage);
    }


    float ExtractPercentage(string text)
    {
        if (text.EndsWith("%") && float.TryParse(text.Replace("%", ""), out float value))
        {
            return Mathf.Clamp(value, 0, maxPercentage);
        }
        return 0; // Default to 0% if parsing fails
    }

    void UpdateTextColor(float percentage)
    {
        float colorRatio = percentage / maxPercentage;
        textMesh.color = Color.Lerp(startColor, endColor, colorRatio);
    }
}
