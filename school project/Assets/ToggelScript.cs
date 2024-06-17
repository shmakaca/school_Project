using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnOffButtonController : MonoBehaviour
{
    public bool isOn; // Button state

    public Button onOffButton; // Reference to the button
    public TMP_Text buttonText; // Reference to the TMP text
    public Image onImage; // Reference to the "On" image
    public Image offImage; // Reference to the "Off" image

    // Colors for the images in different states
    public Color onImageOnColor = Color.white;
    public Color onImageOffColor = Color.gray;
    public Color offImageOnColor = Color.gray;
    public Color offImageOffColor = Color.white;

    void Start()
    {
        // Load the saved state (default is OFF, so default value is 0)
        isOn = PlayerPrefs.GetInt("ButtonState", 0) == 1;

        // Update the UI based on the loaded state
        UpdateUI();

        // Add a listener to the button
        onOffButton.onClick.AddListener(ToggleButton);
    }

    void ToggleButton()
    {
        // Toggle the state
        isOn = !isOn;

        // Update the UI based on the new state
        UpdateUI();

        // Save the state (1 for ON, 0 for OFF)
        PlayerPrefs.SetInt("ButtonState", isOn ? 1 : 0);
    }

    void UpdateUI()
    {
        // Update the button text based on the state
        buttonText.text = isOn ? "ON" : "OFF";

        // Update the image colors based on the state
        if (isOn)
        {
            onImage.color = onImageOnColor;
            offImage.color = offImageOnColor;
        }
        else
        {
            onImage.color = onImageOffColor;
            offImage.color = offImageOffColor;
        }
    }
}
