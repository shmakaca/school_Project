using UnityEngine;
using TMPro; // Import TextMeshPro namespace
using UnityEngine.Events; // Import UnityEvents namespace
using System;
using System.Collections.Generic;

public class KeybindManager : MonoBehaviour
{
    [Serializable]
    public class Keybind
    {
        public string actionName; // Name of the action
        public KeyCode defaultKey; // Default key for the action
        public KeyCode currentKey; // Current key for the action
    }

    public List<Keybind> keybinds = new List<Keybind>(); // List of keybinds

    public TMP_Text buttonTextPrefab; // Prefab for displaying button text using TMP_Text
    public Transform buttonParent; // Parent object for buttons

    private bool waitingForKey = false; // Flag to check if waiting for a key press
    private TMP_Text currentButtonText; // Reference to the button text being modified

    private void Start()
    {
        // Create buttons for each keybind
        foreach (Keybind keybind in keybinds)
        {
            CreateKeybindButton(keybind);
        }
    }

    private void CreateKeybindButton(Keybind keybind)
    {
        // Instantiate the buttonTextPrefab
        TMP_Text buttonText = Instantiate(buttonTextPrefab, buttonParent);

        // Set button text to current key
        buttonText.text = keybind.currentKey.ToString();

        // Add listener to handle key change
        var buttonEvent = new UnityEvent();
        buttonEvent.AddListener(() => { SetNewKey(buttonText, keybind); });
        buttonText.gameObject.AddComponent<Clickable>().OnClick = buttonEvent; // Add Clickable script to handle click events

        // Add listener to handle reset
        // You can handle reset button in a similar way or use a different UI element for reset
    }

    private void SetNewKey(TMP_Text buttonText, Keybind keybind)
    {
        waitingForKey = true;
        currentButtonText = buttonText;
    }

    private void Update()
    {
        if (waitingForKey)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    // Check if the pressed key is not already used for another action
                    if (!IsKeybindAlreadyInUse(keyCode))
                    {
                        // Update keybind and button text
                        currentButtonText.text = keyCode.ToString();
                        foreach (Keybind keybind in keybinds)
                        {
                            if (keybind.currentKey == keybind.defaultKey)
                            {
                                keybind.currentKey = keyCode;
                                break;
                            }
                        }
                    }
                    waitingForKey = false;
                    break;
                }
            }
        }
    }

    private bool IsKeybindAlreadyInUse(KeyCode keyCode)
    {
        foreach (Keybind keybind in keybinds)
        {
            if (keybind.currentKey == keyCode)
            {
                return true;
            }
        }
        return false;
    }
}

// Clickable script to handle click events
public class Clickable : MonoBehaviour
{
    public UnityEvent OnClick;

    private void OnMouseDown()
    {
        OnClick.Invoke();
    }
}
