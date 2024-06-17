using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderValueUpdater : MonoBehaviour
{
    public Slider slider;
    public TMP_InputField inputField;

    private void Start()
    {
        // Initialize input field with slider value
        inputField.text = slider.value.ToString();

        // Add listener to input field to handle value changes
        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);

        // Add listener to slider to handle value changes
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnInputFieldValueChanged(string newValue)
    {
        // If the input field is empty, set slider value to minimum
        if (string.IsNullOrEmpty(newValue))
        {
            slider.value = slider.minValue;
            return;
        }

        // Check if the new value is a valid float
        if (!float.TryParse(newValue, out float value))
        {
            // If not valid, revert to slider value
            inputField.text = slider.value.ToString();
            return;
        }

        // Clamp value within slider's range
        value = Mathf.Clamp(value, slider.minValue, slider.maxValue);

        // Update slider value
        slider.value = value;
    }

    private void OnSliderValueChanged(float newValue)
    {
        // Update input field with slider value
        inputField.text = newValue.ToString();
    }
}
