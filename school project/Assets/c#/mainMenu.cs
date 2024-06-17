using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject SettingsMenu;
    public GameObject PauseMenu;
    public GameObject ControlSidemenu;
    public GameObject SoundSidemenu;
    public GameObject KeybindsMiddleMenu;
    public GameObject MouseMiddleMenu;
    public GameObject SoundMiddleMenu;
    public GameObject Player;

    [Header("Components")]
    public Camera Cam;
    public PlayerMovement PlayerMovementScript;

    [Header("Buttons")]
    public Button ControlsButton;
    public Button SoundButton;
    public Button KeybindsButton;
    public Button MouseButton;
    public Button SoundSettingsButton;

    [Header("Bools")]
    public bool InPauseMenu;

    void Start()
    {

        PauseMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        SoundSidemenu.SetActive(false);
        ControlSidemenu.SetActive(false);
        KeybindsMiddleMenu.SetActive(false);
        MouseMiddleMenu.SetActive(false);
        SoundMiddleMenu.SetActive(false);

        InPauseMenu = false;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        InPauseMenu = !InPauseMenu;

        PauseMenu.SetActive(InPauseMenu);
        PlayerMovementScript.enabled = !InPauseMenu;


        SettingsMenu.SetActive(false);
        SoundSidemenu.SetActive(false);
        ControlSidemenu.SetActive(false);
        KeybindsMiddleMenu.SetActive(false);
        MouseMiddleMenu.SetActive(false);
        SoundMiddleMenu.SetActive(false);

        // Show/hide mouse cursor
        Cursor.visible = InPauseMenu;
        Cursor.lockState = InPauseMenu ? CursorLockMode.None : CursorLockMode.Locked;
    }


    public void OpenSettingsMenu()
    {
        SettingsMenu.SetActive(true);
        PauseMenu.SetActive(false);


        ShowControlsMenu();
        ShowKeybindsMenu();

        ControlsButton.Select();
        KeybindsButton.Select();
    }

    public void ShowControlsMenu()
    {
        ControlSidemenu.SetActive(true);
        SoundSidemenu.SetActive(false);

        ShowKeybindsMenu();

        
        ControlsButton.Select();
    }

   
    public void ShowSoundMenu()
    {
        ControlSidemenu.SetActive(false);
        SoundSidemenu.SetActive(true);

       
        ShowSoundSettings();

       
        SoundButton.Select();
    }

    
    public void ShowKeybindsMenu()
    {
        KeybindsMiddleMenu.SetActive(true);
        MouseMiddleMenu.SetActive(false);
        SoundMiddleMenu.SetActive(false);

        
        KeybindsButton.Select();
    }

    
    public void ShowMouseMenu()
    {
        KeybindsMiddleMenu.SetActive(false);
        MouseMiddleMenu.SetActive(true);
        SoundMiddleMenu.SetActive(false);

        
        MouseButton.Select();
    }


    public void ShowSoundSettings()
    {
        KeybindsMiddleMenu.SetActive(false);
        MouseMiddleMenu.SetActive(false);
        SoundMiddleMenu.SetActive(true);

       
        SoundSettingsButton.Select();
    }

   
    public void ResumeGame()
    {
        TogglePauseMenu();
    }


    public void QuitGame()
    {
        Application.Quit();
    }


    public void LeaveGame()
    {

    }
}
