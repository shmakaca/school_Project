using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class KeybindManager : MonoBehaviour
{
    // Dictionary to store keybinds
    private Dictionary<string, KeyCode> keybinds = new Dictionary<string, KeyCode>();

    // UI Text fields to display current keybinds
    public Text jumpKeyText;
    public Text sprintKeyText;
    // Add more Text fields for other actions as needed

    // Start is called before the first frame update
    void Start()
    {
        // Initialize default keybinds
        keybinds.Add("Jump", KeyCode.Space);
        keybinds.Add("Sprint", KeyCode.LeftShift);
        // Add default keybinds for other actions

        // Load saved keybinds (if any)
        LoadKeybinds();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for key presses to update keybinds
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Open settings menu
        }
    }

    // Function to detect key press and update keybind
    public void ChangeKeybind(string action)
    {
        StartCoroutine(WaitForKey(action));
    }

    IEnumerator WaitForKey(string action)
    {
        // Display a message telling the player to press a key
        // Wait until a key is pressed
        yield return null;

        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    // Update keybind for the specified action
                    keybinds[action] = keyCode;

                    // Update UI text to display the new keybind
                    UpdateKeybindUI(action, keyCode);

                    // Save keybinds
                    SaveKeybinds();

                    break;
                }
            }
        }
    }

    // Function to update UI text for keybinds
    void UpdateKeybindUI(string action, KeyCode keyCode)
    {
        switch (action)
        {
            case "Jump":
                jumpKeyText.text = keyCode.ToString();
                break;
            case "Sprint":
                sprintKeyText.text = keyCode.ToString();
                break;
                // Add cases for other actions as needed
        }
    }

    // Function to save keybinds
    void SaveKeybinds()
    {
        // Use PlayerPref to save keybinds
        // Example: PlayerPrefs.SetInt("JumpKey", (int)keybinds["Jump"]);
    }

    // Function to load saved keybinds
    void LoadKeybinds()
    {
        // Load keybinds from PlayerPrefs
        // Example: keybinds["Jump"] = (KeyCode)PlayerPrefs.GetInt("JumpKey", (int)KeyCode.Space);
    }
}
