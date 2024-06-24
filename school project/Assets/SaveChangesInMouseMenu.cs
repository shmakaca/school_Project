using UnityEngine;
using UnityEngine.UI;

public class MouseSettings : MonoBehaviour
{
    public Slider mouseXSlider;
    public Slider mouseYSlider;

    private const string mouseXKey = "MouseX";
    private const string mouseYKey = "MouseY";

    void Start()
    {
        // Load saved sensitivity values or set default values if not available
        float mouseXValue = PlayerPrefs.GetFloat(mouseXKey, 0.5f);
        float mouseYValue = PlayerPrefs.GetFloat(mouseYKey, 0.5f);

        // Set slider values
        mouseXSlider.value = mouseXValue;
        mouseYSlider.value = mouseYValue;

        // Subscribe to slider value changed events
        mouseXSlider.onValueChanged.AddListener(OnMouseXSliderValueChanged);
        mouseYSlider.onValueChanged.AddListener(OnMouseYSliderValueChanged);
    }

    // Method to handle mouse X slider value change
    private void OnMouseXSliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat(mouseXKey, value);
    }

    // Method to handle mouse Y slider value change
    private void OnMouseYSliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat(mouseYKey, value);
    }

    // Method to get the current sensitivity adjusted for inversion
    public Vector2 GetCurrentSensitivity()
    {
        float sensX = mouseXSlider.value * 0.1f;
        float sensY = mouseYSlider.value * 0.1f;
        return new Vector2(sensX, sensY);
    }
}
