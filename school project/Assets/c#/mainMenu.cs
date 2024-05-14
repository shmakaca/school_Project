using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class mainMenu : MonoBehaviour
{
    [Header("sliders")]
    public Slider soundSlider;
    public Slider FovSlider;

    public GameObject setMenu;
    public GameObject menu;
    public GameObject player ,cam;
    public GameObject KeybindsMenu;

    public bool InPauseMenu;
    public bool InSetMenu;
    public bool InKeyBindMenu;
    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
        setMenu.SetActive(false);
        KeybindsMenu.SetActive(false);
        InPauseMenu = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!InSetMenu)
            {
                PauseMenu();

                 Cursor.visible = true;
                 Cursor.lockState = CursorLockMode.Confined;
            }
            
        }
        
        
    }
    public void PauseMenu() //open main menu
    {
        InSetMenu = false;
        InPauseMenu = true;
        menu.SetActive(true);
        setMenu.SetActive(false);
        player.SetActive(false);
        cam.SetActive(false);
        Time.timeScale = 0f;
    }
    public void settings() //open settings from main menu
    {
        InSetMenu = true;
        setMenu.SetActive(true);
        menu.SetActive(false);
        KeybindsMenu.SetActive(false);
    }

    public void KeyBindsMenu()
    {
        KeybindsMenu.SetActive(true);
        setMenu.SetActive(false);
    }
    public void Resume() //exit GUI
    {
        InPauseMenu = false;
        InSetMenu= false;

        player.SetActive(true);
        cam.SetActive(true);

        menu.SetActive(false);
        setMenu.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        Time.timeScale = 1f;
    }
    
    public void quit() //public function that exit the game (only works on a working game not in unity)
    {
        Application.Quit();
    }
    public float GetVol() //public function that returnes the value of the sound slider from the settings menu
    {
        return soundSlider.value;
    }
    public float GetFov() //public function that returnes the value of the Fov slider from the settings menu
    {
        return FovSlider.value;
    }
}
