using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseSettings : MonoBehaviour
{
    public Slider mouseXSlider;
    public Slider mouseYSlider;
    public Slider invertMouseSlider; // New slider for inverting mouse
    public TextMeshProUGUI mouseXText;
    public TextMeshProUGUI mouseYText;
    public TextMeshProUGUI invertMouseText; // Text for displaying inversion status

    private const string mouseXKey = "MouseX";
    private const string mouseYKey = "MouseY";
    private const string invertMouseKey = "InvertMouse";

    void Start()
    {
        // Load saved sensitivity values or set default values if not available
        float mouseXValue = PlayerPrefs.GetFloat(mouseXKey, 0.5f);
        float mouseYValue = PlayerPrefs.GetFloat(mouseYKey, 0.5f);
        float invertMouseValue = PlayerPrefs.GetFloat(invertMouseKey, 0f); // Default to off

        // Set slider values and text
        mouseXSlider.value = mouseXValue;
        mouseYSlider.value = mouseYValue;
        invertMouseSlider.value = invertMouseValue;
        mouseXText.text = mouseXValue.ToString("F2");
        mouseYText.text = mouseYValue.ToString("F2");
        UpdateInvertMouseText(invertMouseValue);

        // Subscribe to slider value changed events
        mouseXSlider.onValueChanged.AddListener(OnMouseXSliderValueChanged);
        mouseYSlider.onValueChanged.AddListener(OnMouseYSliderValueChanged);
        invertMouseSlider.onValueChanged.AddListener(OnInvertMouseSliderValueChanged);
    }

    // Method to handle mouse X slider value change
    private void OnMouseXSliderValueChanged(float value)
    {
        mouseXText.text = value.ToString("F2");
        PlayerPrefs.SetFloat(mouseXKey, value);
    }

    // Method to handle mouse Y slider value change
    private void OnMouseYSliderValueChanged(float value)
    {
        mouseYText.text = value.ToString("F2");
        PlayerPrefs.SetFloat(mouseYKey, value);
    }

    // Method to handle invert mouse slider value change
    private void OnInvertMouseSliderValueChanged(float value)
    {
        UpdateInvertMouseText(value);
        PlayerPrefs.SetFloat(invertMouseKey, value);
    }

    // Method to update the invert mouse text based on the slider value
    private void UpdateInvertMouseText(float value)
    {
        invertMouseText.text = (value == 0f) ? "Off" : "On";
    }

    // Method to get the current sensitivity adjusted for inversion
    public Vector2 GetCurrentSensitivity()
    {
        float sensX = mouseXSlider.value * 100f;
        float sensY = mouseYSlider.value * 100f;
        if (invertMouseSlider.value == 1f)
        {
            sensX *= -1f;
            sensY *= -1f;
        }
        return new Vector2(sensX, sensY);
    }
}
