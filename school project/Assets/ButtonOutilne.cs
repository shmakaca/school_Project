using UnityEngine;
using UnityEngine.UI;

public class AddCleanOutline : MonoBehaviour
{
    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            // Create a new GameObject for the shadows
            GameObject shadowObject = new GameObject("ButtonShadow");
            shadowObject.transform.SetParent(button.transform, false);

            // Add Shadow components
            AddShadow(shadowObject, new Vector2(-1, 1));  // Top-left
            AddShadow(shadowObject, new Vector2(1, 1));   // Top-right
            AddShadow(shadowObject, new Vector2(-1, -1)); // Bottom-left
            AddShadow(shadowObject, new Vector2(1, -1));  // Bottom-right

            // Ensure the shadow object has the same RectTransform settings as the button
            RectTransform shadowRect = shadowObject.AddComponent<RectTransform>();
            RectTransform buttonRect = button.GetComponent<RectTransform>();
            shadowRect.anchorMin = buttonRect.anchorMin;
            shadowRect.anchorMax = buttonRect.anchorMax;
            shadowRect.sizeDelta = buttonRect.sizeDelta;
            shadowRect.anchoredPosition = buttonRect.anchoredPosition;
        }
    }

    void AddShadow(GameObject shadowObject, Vector2 effectDistance)
    {
        Shadow shadow = shadowObject.AddComponent<Shadow>();
        shadow.effectColor = Color.black; // Set outline color
        shadow.effectDistance = effectDistance; // Set shadow offset
    }
}
