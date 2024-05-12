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
    
    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainMenu();
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        
        
    }
    public void MainMenu() //open main menu
    {
        menu.SetActive(true);
        player.SetActive(false);
        cam.SetActive(false);
    }
    public void settings() //open settings from main menu
    {
        setMenu.SetActive(true);
        menu.SetActive(false);
    }
    public void exit() //exit GUI
    {
        player.SetActive(true);
        cam.SetActive(true);

        menu.SetActive(false);
        setMenu.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

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
