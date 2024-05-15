using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlidersValue : MonoBehaviour
{
    [System.Serializable]
    public struct SliderTextPair
    {
        public Slider slider;
        public TextMeshProUGUI textMeshPro;
    }

    public SliderTextPair[] sliderTextPairs;

    void Start()
    {
        // Subscribe to each slider's OnValueChanged event
        foreach (var pair in sliderTextPairs)
        {
            pair.slider.onValueChanged.AddListener(value => UpdateText(pair.textMeshPro, value));
            // Initialize the text
            UpdateText(pair.textMeshPro, pair.slider.value);
        }
    }

    // This method updates the TextMeshPro text with the current slider value
    void UpdateText(TextMeshProUGUI textMeshPro, float value)
    {
        textMeshPro.text = value.ToString();
    }

    // Unsubscribe from the event when the script is disabled or destroyed
    void OnDestroy()
    {
        foreach (var pair in sliderTextPairs)
        {
            pair.slider.onValueChanged.RemoveAllListeners();
        }
    }

}
