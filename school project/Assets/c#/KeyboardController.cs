using UnityEngine;
using TMPro; // Import TextMeshPro namespace
using UnityEngine.UI; // Import UI namespace
using System;
using System.Collections.Generic;

public class KeybindManager : MonoBehaviour
{
    [Serializable]
    public class Keybind
    {
        public string actionName;
        public Button ResetButton;
        public Button ActionButton;
        public TMP_Text buttonText;
        public KeyCode defaultKey;
        public KeyCode currentKey = KeyCode.None;
    }

    public List<Keybind> keybindsList = new List<Keybind>();
    public Button resetAllButton; // Button to reset all keys
    private bool waitingForKey = false;
    private Keybind currentKeybind;

    void Start()
    {
        LoadKeybinds(); // Load keybindings when the game starts

        foreach (Keybind keybind in keybindsList)
        {
            UpdateButtonText(keybind); // Update the button text to show the current key

            // Add listener to each action button
            keybind.ActionButton.onClick.AddListener(() => OnClick(keybind));

            // Add listener to each reset button
            keybind.ResetButton.onClick.AddListener(() => OnReset(keybind));
        }

        // Add listener to the reset all button
        if (resetAllButton != null)
        {
            resetAllButton.onClick.AddListener(ResetAllKeys);
        }
    }

    void Update()
    {
        if (waitingForKey && Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    currentKeybind.currentKey = keyCode;
                    UpdateButtonText(currentKeybind);
                    SaveKeybind(currentKeybind); // Save the keybinding whenever it is changed
                    waitingForKey = false;
                    break;
                }
            }
        }
    }

    void OnClick(Keybind keybind)
    {
        if (!waitingForKey)
        {
            waitingForKey = true;
            currentKeybind = keybind;
            keybind.buttonText.text = "Press any key...";
        }
    }

    void OnReset(Keybind keybind)
    {
        keybind.currentKey = keybind.defaultKey;
        UpdateButtonText(keybind);
        SaveKeybind(keybind); // Save the keybinding when it is reset
    }

    void UpdateButtonText(Keybind keybind)
    {
        keybind.buttonText.text = keybind.currentKey.ToString();
    }

    void ResetAllKeys()
    {
        foreach (Keybind keybind in keybindsList)
        {
            keybind.currentKey = keybind.defaultKey;
            UpdateButtonText(keybind);
            SaveKeybind(keybind); // Save the keybinding when all are reset
        }
    }

    void SaveKeybind(Keybind keybind)
    {
        PlayerPrefs.SetString(keybind.actionName, keybind.currentKey.ToString());
        PlayerPrefs.Save();
    }

    void LoadKeybinds()
    {
        foreach (Keybind keybind in keybindsList)
        {
            if (PlayerPrefs.HasKey(keybind.actionName))
            {
                try
                {
                    keybind.currentKey = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(keybind.actionName));
                }
                catch
                {
                    // If parsing fails, reset to default key
                    keybind.currentKey = keybind.defaultKey;
                }
            }
            else
            {
                keybind.currentKey = keybind.defaultKey;
            }
            UpdateButtonText(keybind); // Update the button text after loading
        }
    }

    public KeyCode GetKeyCode(string actionName)
    {
        foreach (var keybind in keybindsList)
        {
            if (keybind.actionName == actionName)
            {
                return keybind.currentKey;
            }
        }
        return KeyCode.None;
    }
}
